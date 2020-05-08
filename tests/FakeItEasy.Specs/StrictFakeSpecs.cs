namespace FakeItEasy.Specs
{
    using System;
    using FakeItEasy.Tests.TestHelpers;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class StrictFakeSpecs
    {
        public interface IMyInterface
        {
            void DoIt(Guid id);

            Guid MakeIt(string name);
        }

        [Scenario]
        [InlineData(StrictFakeOptions.AllowEquals)]
        [InlineData(StrictFakeOptions.AllowObjectMethods)]
        public static void CallToEqualsAllowed(
            StrictFakeOptions strictOptions,
            IMyInterface fake,
            Exception exception)
        {
            "Given a strict fake that allows calls to Equals"
                .x(() => fake = A.Fake<IMyInterface>(options =>
                    options.Strict(strictOptions)));

            "When I call Equals on the fake"
                .x(() => exception = Record.Exception(
                    () => fake.Equals(null)));

            "Then it shouldn't throw an exception"
                .x(() => exception.Should().BeNull());
        }

        [Scenario]
        [InlineData(StrictFakeOptions.None)]
        [InlineData(StrictFakeOptions.AllowGetHashCode)]
        [InlineData(StrictFakeOptions.AllowToString)]
        public static void CallToEqualsNotAllowed(
            StrictFakeOptions strictOptions,
            IMyInterface fake,
            Exception exception)
        {
            "Given a strict fake that doesn't allow calls to Equals"
                .x(() => fake = A.Fake<IMyInterface>(options =>
                    options.Strict(strictOptions)));

            "When I call Equals on the fake"
                .x(() => exception = Record.Exception(
                    () => fake.Equals(null)));

            "Then it should throw an exception"
                .x(() => exception.Should().BeAnExceptionOfType<ExpectationException>());
        }

        [Scenario]
        [InlineData(StrictFakeOptions.AllowGetHashCode)]
        [InlineData(StrictFakeOptions.AllowObjectMethods)]
        public static void CallToGetHashCodeAllowed(
            StrictFakeOptions strictOptions,
            IMyInterface fake,
            Exception exception)
        {
            "Given a strict fake that allows calls to GetHashCode"
                .x(() => fake = A.Fake<IMyInterface>(options =>
                    options.Strict(strictOptions)));

            "When I call GetHashCode on the fake"
                .x(() => exception = Record.Exception(
                    () => fake.GetHashCode()));

            "Then it shouldn't throw an exception"
                .x(() => exception.Should().BeNull());
        }

        [Scenario]
        [InlineData(StrictFakeOptions.None)]
        [InlineData(StrictFakeOptions.AllowEquals)]
        [InlineData(StrictFakeOptions.AllowToString)]
        public static void CallToGetHashCodeNotAllowed(
            StrictFakeOptions strictOptions,
            IMyInterface fake,
            Exception exception)
        {
            "Given a strict fake that doesn't allow calls to GetHashCode"
                .x(() => fake = A.Fake<IMyInterface>(options =>
                    options.Strict(strictOptions)));

            "When I call GetHashCode on the fake"
                .x(() => exception = Record.Exception(
                    () => fake.GetHashCode()));

            "Then it should throw an exception"
                .x(() => exception.Should().BeAnExceptionOfType<ExpectationException>());
        }

        [Scenario]
        [InlineData(StrictFakeOptions.AllowToString)]
        [InlineData(StrictFakeOptions.AllowObjectMethods)]
        public static void CallToToStringAllowed(
            StrictFakeOptions strictOptions,
            IMyInterface fake,
            Exception exception)
        {
            "Given a strict fake that allows calls to ToString"
                .x(() => fake = A.Fake<IMyInterface>(options =>
                    options.Strict(strictOptions)));

            "When I call ToString on the fake"
                .x(() => exception = Record.Exception(
                    () => fake.ToString()));

            "Then it shouldn't throw an exception"
                .x(() => exception.Should().BeNull());
        }

        [Scenario]
        [InlineData(StrictFakeOptions.None)]
        [InlineData(StrictFakeOptions.AllowEquals)]
        [InlineData(StrictFakeOptions.AllowGetHashCode)]
        public static void CallToToStringNotAllowed(
            StrictFakeOptions strictOptions,
            IMyInterface fake,
            Exception exception)
        {
            "Given a strict fake that doesn't allow calls to ToString"
                .x(() => fake = A.Fake<IMyInterface>(options =>
                    options.Strict(strictOptions)));

            "When I call ToString on the fake"
                .x(() => exception = Record.Exception(
                    () => fake.ToString()));

            "Then it should throw an exception"
                .x(() => exception.Should().BeAnExceptionOfType<ExpectationException>());
        }

        [Scenario]
        public static void ArgumentMismatchVoid(
            IMyInterface fake,
            Exception exception)
        {
            "Given a strict fake"
                .x(() => fake = A.Fake<IMyInterface>(o => o.Strict()));

            "And I configure the fake to do nothing when a void method is called with certain arguments"
                .x(() => A.CallTo(() => fake.DoIt(Guid.Empty)).DoesNothing());

            "When I call the method with non-matching arguments"
                .x(() => exception = Record.Exception(() => fake.DoIt(new Guid("a762f030-d39e-4316-92b1-a51eff3dc51f"))));

            "Then the fake throws an expectation exception"
                .x(() => exception.Should().BeAnExceptionOfType<ExpectationException>());

            "And the exception message describes the call"
                .x(() => exception.Message.Should().Be(
                    "Call to unconfigured method of strict fake: FakeItEasy.Specs.StrictFakeSpecs+IMyInterface.DoIt(id: a762f030-d39e-4316-92b1-a51eff3dc51f)."));
        }

        [Scenario]
        public static void ArgumentMismatchNonVoid(
            IMyInterface fake,
            Exception exception)
        {
            "Given a strict fake"
                .x(() => fake = A.Fake<IMyInterface>(o => o.Strict()));

            "And I configure the fake to return a value when a non-void method is called with certain arguments"
                .x(() => A.CallTo(() => fake.MakeIt("empty")).Returns(Guid.Empty));

            "When I call the method with non-matching arguments"
                .x(() => exception = Record.Exception(() => fake.MakeIt("something")));

            "Then the fake throws an expectation exception"
                .x(() => exception.Should().BeAnExceptionOfType<ExpectationException>());

            "And the exception message describes the call"
                .x(() => exception.Message.Should().Be(
                    @"Call to unconfigured method of strict fake: FakeItEasy.Specs.StrictFakeSpecs+IMyInterface.MakeIt(name: ""something"")."));
        }

        [Scenario]
        public static void CallCountAssertion(
            IMyInterface fake,
            Exception exception)
        {
            "Given a strict fake"
                .x(() => fake = A.Fake<IMyInterface>(o => o.Strict()));

            "And I configure the fake to do nothing when a method is called with certain arguments"
                .x(() => A.CallTo(() => fake.DoIt(Guid.Empty)).DoesNothing());

            "And I call the method with matching arguments"
                .x(() => fake.DoIt(Guid.Empty));

            "And I call the method with matching arguments again"
                .x(() => fake.DoIt(Guid.Empty));

            "When I assert that the method must have been called exactly once"
                .x(() => exception = Record.Exception(() => A.CallTo(() => fake.DoIt(Guid.Empty)).MustHaveHappenedOnceExactly()));

            "Then the assertion throws an expectation exception"
                .x(() => exception.Should().BeAnExceptionOfType<ExpectationException>());

            "And the exception message names the method"
                .x(() => exception.Message.Should().Contain("DoIt"));
        }
    }
}
