namespace Linn.Api.Ifttt.Service.Validators
{
    using FluentValidation;

    using Linn.Api.Ifttt.Resources.Ifttt;

    public class PlayMediaItemActionFieldResourceValidator 
        : AbstractValidator<ActionRequestResource<PlayMediaItemActionFieldResource>>
    {
        public PlayMediaItemActionFieldResourceValidator()
        {
            this.RuleFor(c => c.ActionFields.Device_Id).NotEmpty().WithMessage("Action field `device_id` missing");
            this.RuleFor(c => c.ActionFields.Media_Url).NotEmpty().WithMessage("Action field `media_url` missing");
        }
    }
}