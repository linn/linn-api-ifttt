using System;
using FluentValidation;
using Linn.Api.Ifttt.Resources.Ifttt;

namespace Linn.Api.Ifttt.Service.Validators
{
    public class SelectSourceActionFieldResourceValidator : AbstractValidator<ActionRequestResource<SelectSourceActionFieldResource>>
    {
        public SelectSourceActionFieldResourceValidator()
        {
            this.RuleFor(c => c.ActionFields).NotNull().WithMessage("`actionFields` missing").DependentRules(
                rules =>
                {
                    rules.RuleFor(c => c.ActionFields.DeviceSource_Id).NotEmpty()
                        .WithMessage("Action field `devicesource_id` missing");
                });
        }
    }
}
