namespace Linn.Api.Ifttt.Service.Validators
{
    using FluentValidation;

    using Linn.Api.Ifttt.Resources.Ifttt;

    public class DeviceActionFieldResourceValidator : AbstractValidator<ActionRequestResource<DeviceActionFieldResource>>
    {
        public DeviceActionFieldResourceValidator()
        {
            this.RuleFor(c => c.ActionFields).NotNull().WithMessage("`actionFields` missing").DependentRules(
                rules =>
                    {
                        rules.RuleFor(c => c.ActionFields.Device_Id).NotEmpty()
                            .WithMessage("Action field `device_id` missing");
                    });
        }
    }
}
