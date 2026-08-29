using System;
using System.Collections.Generic;

namespace DesignPatterns.Creational.Builder.Variants
{
    // Вариант 1: классический Builder. Один Director выполняет одинаковые шаги,
    // а разные строители получают несвязанные продукты.
    public interface IHouseBuilder
    {
        void Reset();
        void BuildWalls();
        void BuildRoof();
        void BuildGarage();
    }

    public sealed class House
    {
        private readonly List<string> _parts = new();
        public IReadOnlyList<string> Parts => _parts.AsReadOnly();
        public void Add(string part) => _parts.Add(part);
    }

    public sealed class HouseBuilder : IHouseBuilder
    {
        private House _house = new();

        public void Reset() => _house = new House();
        public void BuildWalls() => _house.Add("Кирпичные стены");
        public void BuildRoof() => _house.Add("Металлическая крыша");
        public void BuildGarage() => _house.Add("Гараж");

        public House GetResult()
        {
            House result = _house;
            Reset();
            return result;
        }
    }

    public sealed class ConstructionPlan
    {
        private readonly List<string> _steps = new();
        public IReadOnlyList<string> Steps => _steps.AsReadOnly();
        public void Add(string step) => _steps.Add(step);
    }

    public sealed class ConstructionPlanBuilder : IHouseBuilder
    {
        private ConstructionPlan _plan = new();

        public void Reset() => _plan = new ConstructionPlan();
        public void BuildWalls() => _plan.Add("Раздел: расчёт стен");
        public void BuildRoof() => _plan.Add("Раздел: расчёт крыши");
        public void BuildGarage() => _plan.Add("Раздел: расчёт гаража");

        public ConstructionPlan GetResult()
        {
            ConstructionPlan result = _plan;
            Reset();
            return result;
        }
    }

    public sealed class HouseDirector
    {
        public void BuildMinimal(IHouseBuilder builder)
        {
            builder.Reset();
            builder.BuildWalls();
            builder.BuildRoof();
        }

        public void BuildWithGarage(IHouseBuilder builder)
        {
            BuildMinimal(builder);
            builder.BuildGarage();
        }
    }

    // Вариант 2: методы расширения дают fluent-синтаксис существующему
    // изменяемому продукту. Продукт при этом не скрыт до завершения сборки.
    public sealed class MailDraft
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
    }

    public static class MailDraftBuilderExtensions
    {
        public static MailDraft FromAddress(this MailDraft draft, string address)
        {
            draft.From = address;
            return draft;
        }

        public static MailDraft ToAddress(this MailDraft draft, string address)
        {
            draft.To = address;
            return draft;
        }

        public static MailDraft WithSubject(this MailDraft draft, string subject)
        {
            draft.Subject = subject;
            return draft;
        }
    }

    // Вариант 3: строго типизированный (staged/step) Builder. Build появляется
    // только после обязательных шагов To и Subject.
    public interface IRecipientStage
    {
        ISubjectStage To(string address);
    }

    public interface ISubjectStage
    {
        IOptionalStage Subject(string subject);
    }

    public interface IOptionalStage
    {
        IOptionalStage Body(string body);
        StagedEmail Build();
    }

    public sealed class StagedEmail
    {
        internal StagedEmail(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }

        public string To { get; }
        public string Subject { get; }
        public string Body { get; }
    }

    public sealed class StagedEmailBuilder : IRecipientStage, ISubjectStage, IOptionalStage
    {
        private string _to = string.Empty;
        private string _subject = string.Empty;
        private string _body = string.Empty;

        private StagedEmailBuilder()
        {
        }

        public static IRecipientStage Create() => new StagedEmailBuilder();

        public ISubjectStage To(string address)
        {
            _to = address;
            return this;
        }

        public IOptionalStage Subject(string subject)
        {
            _subject = subject;
            return this;
        }

        public IOptionalStage Body(string body)
        {
            _body = body;
            return this;
        }

        public StagedEmail Build() => new(_to, _subject, _body);
    }

    // Вариант 4: вложенный Builder создаёт неизменяемый продукт.
    public sealed class ImmutableRequest
    {
        private ImmutableRequest(Builder builder)
        {
            Url = builder.Url;
            Timeout = builder.Timeout;
            UseCache = builder.UseCache;
        }

        public string Url { get; }
        public TimeSpan Timeout { get; }
        public bool UseCache { get; }

        public static Builder Create() => new();

        public sealed class Builder
        {
            internal string Url { get; private set; } = string.Empty;
            internal TimeSpan Timeout { get; private set; } = TimeSpan.FromSeconds(30);
            internal bool UseCache { get; private set; }

            public Builder To(string url)
            {
                Url = url;
                return this;
            }

            public Builder WithTimeout(TimeSpan timeout)
            {
                Timeout = timeout;
                return this;
            }

            public Builder Cached()
            {
                UseCache = true;
                return this;
            }

            public ImmutableRequest Build()
            {
                if (string.IsNullOrWhiteSpace(Url))
                    throw new InvalidOperationException("URL обязателен.");

                return new ImmutableRequest(this);
            }
        }
    }

    public static class VariantsDemo
    {
        public static void Run()
        {
            var director = new HouseDirector();
            var houseBuilder = new HouseBuilder();
            var planBuilder = new ConstructionPlanBuilder();
            director.BuildWithGarage(houseBuilder);
            director.BuildWithGarage(planBuilder);

            MailDraft draft = new MailDraft()
                .FromAddress("author@example.com")
                .ToAddress("reader@example.com")
                .WithSubject("Builder");

            StagedEmail email = StagedEmailBuilder.Create()
                .To("reader@example.com")
                .Subject("Обязательные шаги проверяет компилятор")
                .Body("Build недоступен до вызовов To и Subject")
                .Build();

            ImmutableRequest request = ImmutableRequest.Create()
                .To("https://example.com")
                .WithTimeout(TimeSpan.FromSeconds(5))
                .Cached()
                .Build();

            Console.WriteLine(houseBuilder.GetResult().Parts.Count);
            Console.WriteLine(planBuilder.GetResult().Steps.Count);
            Console.WriteLine(draft.Subject);
            Console.WriteLine(email.Subject);
            Console.WriteLine(request.Url);
        }
    }
}
