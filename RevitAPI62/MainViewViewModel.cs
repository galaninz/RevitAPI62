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
        public int FamilyQuantity { get; set; }
        public List<XYZ> Points { get; } = new List<XYZ>();
        public FamilySymbol SelectedFurnitureType { get; set; }
        public Level SelectedLevel { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Points = SelectionUtils.GetPoints(_commandData, "Выберите точки", ObjectSnapTypes.Points);
            Levels = LevelsUtils.GetLevels(commandData);
            Furniture = FamilySymbolUtils.GetFamilySymbols(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            FamilyQuantity = 5;


        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            XYZ insertionPoint1 = Points[0];
            XYZ insertionPoint2 = Points[1];

            int ratioX = (int)(insertionPoint1.X / FamilyQuantity - insertionPoint2.X / FamilyQuantity);
            int ratioY = (int)(insertionPoint1.Y / FamilyQuantity - insertionPoint2.Y / FamilyQuantity);
            int ratioZ = (int)(insertionPoint1.Z / FamilyQuantity - insertionPoint2.Z / FamilyQuantity);



            for (int i = 1; i < FamilyQuantity + 1; i++)
            {
                var insertionPoint = new XYZ(ratioX * i, ratioY * i, ratioZ * i);
                FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedFurnitureType, insertionPoint, SelectedLevel);
            }

            RaiseCloseRequest();
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
