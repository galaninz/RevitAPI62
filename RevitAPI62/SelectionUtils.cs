﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI62
{
    public class SelectionUtils
    {
        public static List<Level> Levels { get; internal set; }

        public static Element PickObject(ExternalCommandData commandData, string message = "Выберите элемент")
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var selectedObject = uidoc.Selection.PickObject(ObjectType.Element, message);
            var oElement = doc.GetElement(selectedObject);
            return oElement;
        }

        public static T GetObject<T>(ExternalCommandData commandData, string promptMessage)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Reference selectedObj = null;
            T elem;
            try
            {
                selectedObj = uidoc.Selection.PickObject(ObjectType.Element, promptMessage);
            }
            catch (Exception)
            {
                return default(T);
            }
            elem = (T)(object)doc.GetElement(selectedObj.ElementId);
            return elem;
        }

        public static List<Element> PickObjects(ExternalCommandData commandData, string message = "Выберите элементы")
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var selectedObjects = uidoc.Selection.PickObjects(ObjectType.Element, message);
            List<Element> elementList = selectedObjects.Select(selectedObject => doc.GetElement(selectedObject)).ToList();
            return elementList;
        }

        public static List<XYZ> GetPoints(ExternalCommandData commandData,
                            string promptMessage, ObjectSnapTypes objectSnapTypes)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;

            List<XYZ> points = new List<XYZ>();

            while (points.Count < 1)
            {
                XYZ pickedPoint = null;
                try
                {
                    pickedPoint = uidoc.Selection.PickPoint(objectSnapTypes, promptMessage);
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException ex)
                {
                    break;
                }
                points.Add(pickedPoint);
            }

            return points;
        }
    }
}
