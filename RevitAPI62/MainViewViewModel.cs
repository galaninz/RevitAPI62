using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI62
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<Level> Levels { get; } = new List<Level>();
        public List<FamilySymbol> Furniture { get; } = new List<FamilySymbol>();
        public DelegateCommand SaveCommand { get; }
        public List<XYZ> Points { get; } = new List<XYZ>();
        public FamilySymbol SelectedFurnitureType { get; set; }
        public Level SelectedLevel { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Points = SelectionUtils.GetPoints(_commandData, "Выберите точку", ObjectSnapTypes.Points);
            Levels = LevelsUtils.GetLevels(commandData);
            Furniture = FamilySymbolUtils.GetFamilySymbols(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            
        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            XYZ insertionPoint = Points[0];

            FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedFurnitureType, insertionPoint, SelectedLevel);

            RaiseCloseRequest();
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
