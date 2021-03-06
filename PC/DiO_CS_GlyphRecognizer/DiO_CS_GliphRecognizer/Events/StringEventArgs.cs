﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiO_CS_GliphRecognizer.Events
{
    public class StringEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public StringEventArgs()
        {
            this.Message = String.Empty;
        }

        public StringEventArgs(string message)
        {
            this.Message = message;
        }
    }
}