using System;
using System.Collections.Generic;

namespace DesignPatterns.Behavioral.Mediator
{
    // Интерфейс посредника - через него компоненты общаются друг с другом,
    // не зная напрямую друг о друге.
    public interface IDialogMediator
    {
        void Notify(object sender, string @event);
    }

    // Компоненты диалога. Каждый знает только о посреднике, а не о других компонентах.
    public sealed class CheckoutButton
    {
        private readonly IDialogMediator _mediator;
        public bool IsEnabled { get; set; }

        public CheckoutButton(IDialogMediator mediator) => _mediator = mediator;

        public void Click()
        {
            if (!IsEnabled)
            {
                Console.WriteLine("Кнопка оформления заказа недоступна");
                return;
            }

            Console.WriteLine("Оформляем заказ...");
        }
    }

    public sealed class AgreementCheckbox
    {
        private readonly IDialogMediator _mediator;
        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                _mediator.Notify(this, nameof(IsChecked));
            }
        }

        public AgreementCheckbox(IDialogMediator mediator) => _mediator = mediator;
    }

    public sealed class QuantityInput
    {
        private readonly IDialogMediator _mediator;
        private int _quantity;

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                _mediator.Notify(this, nameof(Quantity));
            }
        }

        public QuantityInput(IDialogMediator mediator) => _mediator = mediator;
    }

    // Конкретный посредник знает обо всех компонентах диалога и содержит логику
    // их согласованного взаимодействия. Сами компоненты об этой логике не знают.
    public sealed class CheckoutDialog : IDialogMediator
    {
        private readonly AgreementCheckbox _agreement;
        private readonly QuantityInput _quantity;
        private readonly CheckoutButton _checkoutButton;

        public CheckoutDialog()
        {
            _agreement = new AgreementCheckbox(this);
            _quantity = new QuantityInput(this);
            _checkoutButton = new CheckoutButton(this);
        }

        public AgreementCheckbox Agreement => _agreement;
        public QuantityInput Quantity => _quantity;
        public CheckoutButton CheckoutButton => _checkoutButton;

        public void Notify(object sender, string @event)
        {
            // Здесь сосредоточена вся логика согласования компонентов между собой
            _checkoutButton.IsEnabled = _agreement.IsChecked && _quantity.Quantity > 0;
            Console.WriteLine($"[Mediator] Состояние обновлено: кнопка доступна = {_checkoutButton.IsEnabled}");
        }
    }

    public static class Demo
    {
        public static void Run()
        {
            var dialog = new CheckoutDialog();

            dialog.CheckoutButton.Click(); // недоступна

            dialog.Quantity.Quantity = 2;
            dialog.CheckoutButton.Click(); // всё ещё недоступна - не принято соглашение

            dialog.Agreement.IsChecked = true;
            dialog.CheckoutButton.Click(); // теперь доступна
        }
    }
}
