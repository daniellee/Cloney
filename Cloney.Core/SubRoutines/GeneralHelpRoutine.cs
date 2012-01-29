﻿using System.Collections.Generic;
using NExtra;
using NExtra.Localization;

namespace Cloney.Core.SubRoutines
{
    /// <summary>
    /// This routine will run when the argument collection
    /// contains a help key with the value "true". It will
    /// then print general Cloney help information.
    /// </summary>
    public class GeneralHelpRoutine : SubRoutineBase, ISubRoutine
    {
        private readonly IConsole console;
        private readonly ITranslator translator;


        public GeneralHelpRoutine()
            :this(new ConsoleFacade(), new LanguageProvider())
        {
        }

        public GeneralHelpRoutine(IConsole console, ITranslator translator)
        {
            this.console = console;
            this.translator = translator;
        }


        public void Run(IDictionary<string, string> args)
        {
            if (args.ContainsKey("help") && args["help"] == "true")
                console.WriteLine(translator.Translate("GeneralHelpMessage"));

            Finish();
        }
    }
}