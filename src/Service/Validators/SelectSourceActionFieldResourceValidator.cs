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
                    rules.RuleFor(c => c.ActionFields.Device_Id).NotEmpty()
                        .WithMessage("Action field `device_id` missing");
                    rules.RuleFor(c => c.ActionFields.Source_Id).NotEmpty()
                        .WithMessage("Action field `source_id` missing");
                });
        }
    }
}
