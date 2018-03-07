namespace Linn.Api.Ifttt.Service.Validators
{
    using FluentValidation;

    using Linn.Api.Ifttt.Resources.Ifttt;

    public class PlayPlaylistActionFieldResourceValidator 
        : AbstractValidator<ActionRequestResource<PlayPlaylistActionFieldResource>>
    {
        public PlayPlaylistActionFieldResourceValidator()
        {
            this.RuleFor(c => c.ActionFields).NotNull().WithMessage("`actionFields` missing").DependentRules(
                rules =>
                    {
                        rules.RuleFor(c => c.ActionFields.Device_Id).NotEmpty()
                            .WithMessage("Action field `device_id` missing");
                        rules.RuleFor(c => c.ActionFields.Playlist_Id).NotEmpty()
                            .WithMessage("Action field `playlist_id` missing");
                    });
        }
    }
}