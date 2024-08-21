using FluentValidation;
using UKHO.S100PermitService.Common.Models;

namespace UKHO.S100PermitService.Common.Validators
{
    public class UserPermitValidator : AbstractValidator<S100UserPermit>
    {
        public UserPermitValidator()
        {
            RuleFor(userPermit => userPermit.MId).NotNull().Length(6);
            RuleFor(userPermit => userPermit.MKey).NotNull().Length(32);
            RuleFor(userPermit => userPermit.HwId).NotNull().Length(32);
            //RuleFor(userPermit => userPermit.UserPermit).Length(46).When(userPermit => string.IsNullOrEmpty(userPermit.UserPermit));
        }
    }

    public class DecryptUserPermitValidator : AbstractValidator<S100UserPermit>
    {
        public DecryptUserPermitValidator()
        {
            RuleFor(userPermit => userPermit.MKey).NotNull().Length(32);
            RuleFor(userPermit => userPermit.UserPermit).Length(46);
        }
        
    }
}
