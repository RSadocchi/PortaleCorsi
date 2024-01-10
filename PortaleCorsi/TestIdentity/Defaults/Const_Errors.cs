using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestIdentity
{
    public partial class Const_Errors
    {
        public const string USER_CREDENTIALS = nameof(USER_CREDENTIALS);
        public const string USER_LOCKEDOUT = nameof(USER_LOCKEDOUT);
        public const string USER_DISABLED = nameof(USER_DISABLED);
        public const string USER_EMAIL_UNCONFIRMED = nameof(USER_EMAIL_UNCONFIRMED);
        public const string USER_PHONENUMBER_UNCONFIRMED = nameof(USER_PHONENUMBER_UNCONFIRMED);

        public const string USER_EXISTS = "U001";
        public const string USER_MISSING = "U002";

        public const string PASSWORD_VALIDATION_FAIL = "PV000";
        public const string PASSWORD_EMPTY = "PV001";
        public const string PASSWORD_SPACES = "PV002";
        public const string PASSWORD_LENGTH = "PV003";
        public const string PASSWORD_MISSING_UPPERCASE = "PV004";
        public const string PASSWORD_MISSING_LOWERCASE = "PV005";
        public const string PASSWORD_MISSING_DIGIT = "PV006";
        public const string PASSWORD_MISSING_NON_ALPHANUMERIC = "PV007";
        public const string PASSWORD_HISTORY = "PV008";

    }
}
