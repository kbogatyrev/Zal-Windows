using System;
using System.Windows.Input;
using System.Collections.Generic;

using MainLibManaged;
using System.Windows;
using System.ComponentModel;

namespace ZalTestApp
{
    public class ParsedTextViewModel : ViewModelBase
    {
        #region ICommand
        #endregion

        public string ParsedText { get; set; }

        public ParsedTextViewModel(MainModel m)
        {
            ParsedText = m.ParsedText;
        }


    }       //  public class ParsedTextViewModel ...
}
