using Core;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Verse3.Elements;
using XamlReader = System.Windows.Markup.XamlReader;
using Verse3.Components;
using Verse3.Nodes;

namespace Verse3
{
    #region DataTemplateManager
    public class DataTemplateManager
    {

        #region Binding Management

        public static void CreateBinding(FrameworkElementFactory BindTo, DependencyProperty BindToProperty, PropertyPath BindFromProperty, BindingMode Mode)
        {
            Binding binding = new Binding();
            binding.Path = BindFromProperty;
            binding.Mode = Mode;
            BindTo.SetBinding(BindToProperty, binding);
        }

        public static void CreateBinding(DependencyObject BindTo, DependencyProperty BindToProperty, object BindFrom, PropertyPath BindFromProperty, BindingMode Mode = BindingMode.TwoWay)
        {
            Binding binding = new Binding();
            binding.Source = BindFrom;
            binding.Path = BindFromProperty;
            binding.Mode = Mode;
            BindingOperations.SetBinding(BindTo, BindToProperty, binding);
        }

        #endregion

        #region Private DataTemplate Management

        //private static void RegisterDataTemplate<TViewModel, TView>() where TView : FrameworkElement
        //{
        //    RegisterDataTemplate(typeof(TViewModel), typeof(TView));
        //}

        //private static void RegisterDataTemplate(Type viewModelType, Type viewType)
        //{
        //    var template = CreateTemplate(viewModelType, viewType);

        //    if (DataViewModel.WPFControl.Resources[template.DataTemplateKey] != null) return;
        //    else
        //    {
        //        DataViewModel.WPFControl.Resources.Add(template.DataTemplateKey, template);
        //    }
        //}

        private static DataTemplate CreateTemplate(Type viewModelType, Type viewType)
        {
            const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";
            var xaml = String.Format(xamlTemplate, viewModelType.Name, viewType.Name, viewModelType.Namespace, viewType.Namespace);

            var context = new ParserContext();

            context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
            context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("vm", "vm");
            context.XmlnsDictionary.Add("v", "v");

            if (xaml.Contains("`1"))
            {
                if (/*viewModelType.GenericTypeArguments.Length == 1 && */viewModelType.IsAssignableTo(typeof(DataNodeElement<>)))
                {
                    //TODO: Log to Console
                    string t = viewModelType.Name;
                    while (xaml.Contains("DataNodeElement`1"))
                    {
                        xaml = xaml.Replace("DataNodeElement`1", t);
                        //xaml = xaml.Replace("`1", (""));
                    }
                }
            }
            //if (/*viewModelType.GenericTypeArguments.Length == 1 && */viewModelType.BaseType == (typeof(EventNodeElement)))
            //{
            //    //TODO: Log to Console
            //    string t = viewModelType.Name;
            //    while (xaml.Contains("EventNodeElement"))
            //    {
            //        xaml = xaml.Replace("EventNodeElement", t);
            //        //xaml = xaml.Replace("`1", (""));
            //    }
            //}
            if (/*viewModelType.GenericTypeArguments.Length == 1 && */viewModelType.BaseType == (typeof(BaseCompViewModel)))
            {
                //TODO: Log to Console
                string t = viewModelType.Name;
                //while (xaml.Contains("BaseComp"))
                //{
                //    //xaml = xaml.Replace("BaseComp", t);
                //    //xaml = xaml.Replace("`1", (""));
                //}
            }

            DataTemplate template = (DataTemplate)XamlReader.Parse(xaml, context);

            return template;

        }

        #endregion

        #region Public DataTemplate Management

        //TODO: Allow calling this method from another thread
        public static bool RegisterDataTemplate(IRenderable el)
        {
            if (el == null) return false;
            if (el.ViewType == null) return false;
            var template = CreateTemplate(el.GetType(), el.ViewType);
            //el.BoundingBox = new BoundingBox();
            //Element needs to know DataTemplateKey in order to make a reference to it
            el.ViewKey = template.DataTemplateKey;
            if (MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView == null) return false;
            if (MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.Resources[el.ViewKey] != null)
            {
                if (MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.Resources.Contains(el.ViewKey)) return false;
                if (el.ViewType.IsAssignableTo(typeof(DataNodeElementModelView)))
                {
                    MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.Resources.Add(el.ViewKey, template);
                    return true;
                }
                else if (el.ViewType.IsAssignableTo(typeof(EventNodeElementModelView)))
                {
                    MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.Resources.Add(el.ViewKey, template);
                    return true;
                }
                return false;
            }
            else
            {
                try
                {
                    Action addTemplate = () =>
                    {
                        MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.Resources.Add(el.ViewKey, template);
                    };
                    MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.Dispatcher.Invoke(addTemplate);
                    //ERROR: The calling thread cannot access this object because a different thread owns it.
                    //DataViewModel.WPFControl.Resources.Add(el.ViewKey, template);
                    return true;
                }
                catch (Exception ex)
                {
                    CoreConsole.Log(ex);
                    return false;
                }
            }
        }

        #endregion
    }

    #endregion
}
