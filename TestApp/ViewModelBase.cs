/*
    Based on Jeremy Alles' blog entry: 
        http://www.japf.fr/2009/03/thinking-with-mvvm-data-templates-contentcontrol/
    
        see also https://joshsmithonwpf.wordpress.com/
 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace ZalTestApp
{
    /// <summary>
    /// Base class for all ViewModel classes in the application. Provides support for 
    /// property changes notification. Original implementation by Josh Smith.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected Guid m_InstanceId;

        public ViewModelBase()
        {
            m_InstanceId = Guid.NewGuid();
        }

        public string InstanceId
        {
            get
            {
                return m_InstanceId.ToString();
            }
        }

        bool m_bIsDerived = false;
        public bool IsDerived
        {
            get
            {
                return m_bIsDerived;
            }

            set
            {
                m_bIsDerived = value;
            }
        }

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Warns the developer if this object does not have a public property with
        /// the specified name. This method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Debug.Fail("Invalid property name: " + propertyName);
            }
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        ////////////////////////////////////////////////////////
        /*
        //       public delegate bool ChangedFormHandler();
                public struct FormDescriptor
                {
                    public List<string> listForms { get; set; }
                    public List<Tuple<string, string>> listComments { get; set; }   // leading / trailing comments, irregular form only
                    //            public bool bCanEdit { get; set; }
        //            public ChangedFormHandler handler { get; set; }
                    public bool IsIrregular { get; set; }
                    public bool IsEdited { get; set; }
                    public int iCurrentFormNumber { get; set; }     // 0 unless we have more than 1 form per gramm hash

        //            public FormDescriptor(List<string> forms, List<Tuple<string, string>> comments, bool bIrregular, bool bEdited, ChangedFormHandler h)
                    public FormDescriptor(List<string> forms, List<Tuple<string, string>> comments, bool bIrregular, bool bEdited)
                    {
                        listForms = forms;
                        listComments = comments;
                        IsIrregular = bIrregular;
                        IsEdited = bEdited;
        //                handler = h;
                        iCurrentFormNumber = 0;
                    }
                }
        */
    }

    public class FormDescriptor
    {
        public string sFormText;
        public string sComment;                            // leading / trailing comments, irregular form only
        public bool HasTrailingComment;
        public bool IsIrregular;
        public bool IsEdited;

        public FormDescriptor()
        {
            sFormText = "";
            sComment = "";
            HasTrailingComment = false;
            IsIrregular = false;
            IsEdited = false;
        }
    }

    // List of forms for given gram hash + sequential number of currently displayed form
    public class FormsForGramHash
    {
        public List<FormDescriptor> listForms;
        public int iCurrentForm;

        public FormsForGramHash()
        {
            listForms = new List<FormDescriptor>();
            iCurrentForm = -1;
        }
    }

    public class RelayCommand : ICommand
    {
        private Action<object> m_Action;
        public RelayCommand(Action<object> action)
        {
            m_Action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                m_Action(parameter);
            }
            else
            {
                m_Action("");
            }
        }
    }

}
