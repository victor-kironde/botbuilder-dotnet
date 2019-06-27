﻿// Licensed under the MIT License.
// Copyright (c) Microsoft Corporation. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive.Steps
{
    /// <summary>
    /// Step which begins executing another dialog, when it is done, it will return to the caller
    /// </summary>
    public class BeginDialog : BaseInvokeDialog
    {
        /// <summary>
        /// Property which is bidirectional property for input and output.  Example: user.age will be passed in, and user.age will be set when the dialog completes
        /// </summary>
        public string Property
        {
            get
            {
                return OutputBinding;
            }
            set
            {
                InputBindings["value"] = value;
                OutputBinding = value;
            }
        }

        [JsonConstructor]
        public BeginDialog(string dialogIdToCall = null, string property = null, object options = null, [CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
            : base(dialogIdToCall, property, options)
        {
            this.RegisterSourceLocation(callerPath, callerLine);
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (options is CancellationToken)
            {
                throw new ArgumentException($"{nameof(options)} cannot be a cancellation token");
            }

            Options = ObjectPath.Merge(Options, options ?? new object());
            var dialog = this.resolveDialog(dc);
            return await dc.BeginDialogAsync(dialog.Id, Options, cancellationToken).ConfigureAwait(false);
        }
    }
}