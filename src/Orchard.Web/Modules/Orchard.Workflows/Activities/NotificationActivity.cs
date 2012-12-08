﻿using System;
using System.Collections.Generic;
using Orchard.Localization;
using Orchard.Tokens;
using Orchard.UI.Notify;
using Orchard.Workflows.Models.Descriptors;
using Orchard.Workflows.Services;

namespace Orchard.Workflows.Activities {
    public class NotificationActivity : BaseActivity {
        private readonly INotifier _notifier;
        private readonly ITokenizer _tokenizer;

        public NotificationActivity(INotifier notifier, ITokenizer tokenizer) {
            _notifier = notifier;
            _tokenizer = tokenizer;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override string Name {
            get { return "Notify"; }
        }

        public override LocalizedString Category {
            get { return T("Notification"); }
        }

        public override LocalizedString Description {
            get { return T("Display a message.");  }
        }

        public override string Form {
            get { return "ActivityNotify"; }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(ActivityContext context) {
            yield return T("Done");
        }

        public override LocalizedString Execute(ActivityContext context) {
            string notification = context.State.Notification;
            string message = context.State.Message;

            message = _tokenizer.Replace(message, context.Tokens);

            var notificationType = (NotifyType)Enum.Parse(typeof(NotifyType), notification);
            _notifier.Add(notificationType, T(message));

            return T("Done");
        }
    }
}