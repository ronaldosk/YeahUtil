using System;
using System.Xml;
using YeahException;
using System.Collections.Generic;

namespace YeahTools.EMS
{
	public class ProduceValues
	{
		public string ClassName {get{ return "ProduceValues";}}
		public ProduceValues()
		{
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
			}
		}
	}
	public class Edge
	{
		public string ClassName {get{ return "Edge";}}
		public string Face {get;set;}
		public string LineID {get;set;}
		public string Thickness {get;set;}
		public string EdgeType {get;set;}
		public string Length {get;set;}
		public string Pre_Milling {get;set;}
		public string X {get;set;}
		public string Y {get;set;}
		public string CentralAngle {get;set;}
		public Edge()
		{
			Face = "";
			LineID = "";
			Thickness = "";
			EdgeType = "";
			Length = "";
			Pre_Milling = "";
			X = "";
			Y = "";
			CentralAngle = "";
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
				this.Face = xmln.Attributes["Face"] == null ? "" : xmln.Attributes["Face"].Value;
				this.LineID = xmln.Attributes["LineID"] == null ? "" : xmln.Attributes["LineID"].Value;
				this.Thickness = xmln.Attributes["Thickness"] == null ? "" : xmln.Attributes["Thickness"].Value;
				this.EdgeType = xmln.Attributes["EdgeType"] == null ? "" : xmln.Attributes["EdgeType"].Value;
				this.Length = xmln.Attributes["Length"] == null ? "" : xmln.Attributes["Length"].Value;
				this.Pre_Milling = xmln.Attributes["Pre_Milling"] == null ? "" : xmln.Attributes["Pre_Milling"].Value;
				this.X = xmln.Attributes["X"] == null ? "" : xmln.Attributes["X"].Value;
				this.Y = xmln.Attributes["Y"] == null ? "" : xmln.Attributes["Y"].Value;
				this.CentralAngle = xmln.Attributes["CentralAngle"] == null ? "" : xmln.Attributes["CentralAngle"].Value;
			}
		}
	}
	public class EdgeGroup
	{
		public string ClassName {get{ return "EdgeGroup";}}
		public string X1 {get;set;}
		public string Y1 {get;set;}
		public List<Edge> edgeList = new List<Edge>();
		public EdgeGroup()
		{
			X1 = "";
			Y1 = "";
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
				this.X1 = xmln.Attributes["X1"] == null ? "" : xmln.Attributes["X1"].Value;
				this.Y1 = xmln.Attributes["Y1"] == null ? "" : xmln.Attributes["Y1"].Value;
			}
		}
	}
	public class Machining
	{
		public string ClassName {get{ return "Machining";}}
		public string ID {get;set;}
		public string IsGenCode {get;set;}
		public string Type {get;set;}
		public string Face {get;set;}
		public string X {get;set;}
		public string Y {get;set;}
		public string Z {get;set;}
		public string HoleType {get;set;}
		public string Diameter {get;set;}
		public string Depth {get;set;}
		public string ToolName {get;set;}
		public string ToolOffset {get;set;}
		public string EdgeABThickness {get;set;}
		public string GrooveType {get;set;}
		public List<Lines> linesList = new List<Lines>();
		public Machining()
		{
			ID = "";
			IsGenCode = "";
			Type = "";
			Face = "";
			X = "";
			Y = "";
			Z = "";
			HoleType = "";
			Diameter = "";
			Depth = "";
			ToolName = "";
			ToolOffset = "";
			EdgeABThickness = "";
			GrooveType = "";
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
				this.ID = xmln.Attributes["ID"] == null ? "" : xmln.Attributes["ID"].Value;
				this.IsGenCode = xmln.Attributes["IsGenCode"] == null ? "" : xmln.Attributes["IsGenCode"].Value;
				this.Type = xmln.Attributes["Type"] == null ? "" : xmln.Attributes["Type"].Value;
				this.Face = xmln.Attributes["Face"] == null ? "" : xmln.Attributes["Face"].Value;
				this.X = xmln.Attributes["X"] == null ? "" : xmln.Attributes["X"].Value;
				this.Y = xmln.Attributes["Y"] == null ? "" : xmln.Attributes["Y"].Value;
				this.Z = xmln.Attributes["Z"] == null ? "" : xmln.Attributes["Z"].Value;
				this.HoleType = xmln.Attributes["HoleType"] == null ? "" : xmln.Attributes["HoleType"].Value;
				this.Diameter = xmln.Attributes["Diameter"] == null ? "" : xmln.Attributes["Diameter"].Value;
				this.Depth = xmln.Attributes["Depth"] == null ? "" : xmln.Attributes["Depth"].Value;
				this.ToolName = xmln.Attributes["ToolName"] == null ? "" : xmln.Attributes["ToolName"].Value;
				this.ToolOffset = xmln.Attributes["ToolOffset"] == null ? "" : xmln.Attributes["ToolOffset"].Value;
				this.EdgeABThickness = xmln.Attributes["EdgeABThickness"] == null ? "" : xmln.Attributes["EdgeABThickness"].Value;
				this.GrooveType = xmln.Attributes["GrooveType"] == null ? "" : xmln.Attributes["GrooveType"].Value;
			}
		}
	}
	public class Machines
	{
		public string ClassName {get{ return "Machines";}}
		public List<Machining> machiningList = new List<Machining>();
		public Machines()
		{
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
			}
		}
	}
	public class Panel
	{
		public string ClassName {get{ return "Panel";}}
		public string PositionNumber {get;set;}
		public string IsProduce {get;set;}
		public string Thickness {get;set;}
		public string CraftMark {get;set;}
		public string ProduceNumber {get;set;}
		public string SubType {get;set;}
		public string Length {get;set;}
		public string Width {get;set;}
		public string ID {get;set;}
		public string Name {get;set;}
		public string Material {get;set;}
		public string MaterialId {get;set;}
		public string BaseMaterialCategoryId {get;set;}
		public string MaterialCategoryId {get;set;}
		public string Model {get;set;}
		public string CabinetType {get;set;}
		public string Type {get;set;}
		public string edgeMaterial {get;set;}
		public string StandardCategory {get;set;}
		public string IsAccurate {get;set;}
		public string MachiningPoint {get;set;}
		public string Grain {get;set;}
		public string ProdutionNo {get;set;}
		public string ProductionName {get;set;}
		public string Face5ID {get;set;}
		public string Face6ID {get;set;}
		public string clerk {get;set;}
		public string PkgNo {get;set;}
		public string BasicMaterial {get;set;}
		public string PartNumber {get;set;}
		public string DoorDirection {get;set;}
		public string Category {get;set;}
		public string BasicMaterialCode {get;set;}
		public string BasicMaterialMCode {get;set;}
		public string BasicMaterialColorNumber {get;set;}
		public string BasicMaterialColorCode {get;set;}
		public string EdgeThickCodeNo {get;set;}
		public string EdgeThickName {get;set;}
		public string EdgeThickMaterialCode {get;set;}
		public string EdgeThickWidth {get;set;}
		public string EdgeThickThickness {get;set;}
		public string EdgeThickType {get;set;}
		public string EdgeThinCodeNo {get;set;}
		public string EdgeThinName {get;set;}
		public string EdgeThinMaterialCode {get;set;}
		public string EdgeThinWidth {get;set;}
		public string EdgeThinThickness {get;set;}
		public string EdgeThinType {get;set;}
		public string EdgeGroupType {get;set;}
		public string thickLength {get;set;}
		public string thinLength {get;set;}
		public string customLength {get;set;}
		public string HasHorizontalHole {get;set;}
		public string ActualLength {get;set;}
		public string ActualWidth {get;set;}
		public string Series {get;set;}
		public string priceP {get;set;}
		public string CabinetNo {get;set;}
		public string CabinetPanelNo {get;set;}
		public List<ProduceValues> producevaluesList = new List<ProduceValues>();
		public List<EdgeGroup> edgegroupList = new List<EdgeGroup>();
		public List<Machines> machinesList = new List<Machines>();
		public List<Handles> handlesList = new List<Handles>();
		public Panel()
		{
			PositionNumber = "";
			IsProduce = "";
			Thickness = "";
			CraftMark = "";
			ProduceNumber = "";
			SubType = "";
			Length = "";
			Width = "";
			ID = "";
			Name = "";
			Material = "";
			MaterialId = "";
			BaseMaterialCategoryId = "";
			MaterialCategoryId = "";
			Model = "";
			CabinetType = "";
			Type = "";
			edgeMaterial = "";
			StandardCategory = "";
			IsAccurate = "";
			MachiningPoint = "";
			Grain = "";
			ProdutionNo = "";
			ProductionName = "";
			Face5ID = "";
			Face6ID = "";
			clerk = "";
			PkgNo = "";
			BasicMaterial = "";
			PartNumber = "";
			DoorDirection = "";
			Category = "";
			BasicMaterialCode = "";
			BasicMaterialMCode = "";
			BasicMaterialColorNumber = "";
			BasicMaterialColorCode = "";
			EdgeThickCodeNo = "";
			EdgeThickName = "";
			EdgeThickMaterialCode = "";
			EdgeThickWidth = "";
			EdgeThickThickness = "";
			EdgeThickType = "";
			EdgeThinCodeNo = "";
			EdgeThinName = "";
			EdgeThinMaterialCode = "";
			EdgeThinWidth = "";
			EdgeThinThickness = "";
			EdgeThinType = "";
			EdgeGroupType = "";
			thickLength = "";
			thinLength = "";
			customLength = "";
			HasHorizontalHole = "";
			ActualLength = "";
			ActualWidth = "";
			Series = "";
			priceP = "";
			CabinetNo = "";
			CabinetPanelNo = "";
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
				this.PositionNumber = xmln.Attributes["PositionNumber"] == null ? "" : xmln.Attributes["PositionNumber"].Value;
				this.IsProduce = xmln.Attributes["IsProduce"] == null ? "" : xmln.Attributes["IsProduce"].Value;
				this.Thickness = xmln.Attributes["Thickness"] == null ? "" : xmln.Attributes["Thickness"].Value;
				this.CraftMark = xmln.Attributes["CraftMark"] == null ? "" : xmln.Attributes["CraftMark"].Value;
				this.ProduceNumber = xmln.Attributes["ProduceNumber"] == null ? "" : xmln.Attributes["ProduceNumber"].Value;
				this.SubType = xmln.Attributes["SubType"] == null ? "" : xmln.Attributes["SubType"].Value;
				this.Length = xmln.Attributes["Length"] == null ? "" : xmln.Attributes["Length"].Value;
				this.Width = xmln.Attributes["Width"] == null ? "" : xmln.Attributes["Width"].Value;
				this.ID = xmln.Attributes["ID"] == null ? "" : xmln.Attributes["ID"].Value;
				this.Name = xmln.Attributes["Name"] == null ? "" : xmln.Attributes["Name"].Value;
				this.Material = xmln.Attributes["Material"] == null ? "" : xmln.Attributes["Material"].Value;
				this.MaterialId = xmln.Attributes["MaterialId"] == null ? "" : xmln.Attributes["MaterialId"].Value;
				this.BaseMaterialCategoryId = xmln.Attributes["BaseMaterialCategoryId"] == null ? "" : xmln.Attributes["BaseMaterialCategoryId"].Value;
				this.MaterialCategoryId = xmln.Attributes["MaterialCategoryId"] == null ? "" : xmln.Attributes["MaterialCategoryId"].Value;
				this.Model = xmln.Attributes["Model"] == null ? "" : xmln.Attributes["Model"].Value;
				this.CabinetType = xmln.Attributes["CabinetType"] == null ? "" : xmln.Attributes["CabinetType"].Value;
				this.Type = xmln.Attributes["Type"] == null ? "" : xmln.Attributes["Type"].Value;
				this.edgeMaterial = xmln.Attributes["edgeMaterial"] == null ? "" : xmln.Attributes["edgeMaterial"].Value;
				this.StandardCategory = xmln.Attributes["StandardCategory"] == null ? "" : xmln.Attributes["StandardCategory"].Value;
				this.IsAccurate = xmln.Attributes["IsAccurate"] == null ? "" : xmln.Attributes["IsAccurate"].Value;
				this.MachiningPoint = xmln.Attributes["MachiningPoint"] == null ? "" : xmln.Attributes["MachiningPoint"].Value;
				this.Grain = xmln.Attributes["Grain"] == null ? "" : xmln.Attributes["Grain"].Value;
				this.ProdutionNo = xmln.Attributes["ProdutionNo"] == null ? "" : xmln.Attributes["ProdutionNo"].Value;
				this.ProductionName = xmln.Attributes["ProductionName"] == null ? "" : xmln.Attributes["ProductionName"].Value;
				this.Face5ID = xmln.Attributes["Face5ID"] == null ? "" : xmln.Attributes["Face5ID"].Value;
				this.Face6ID = xmln.Attributes["Face6ID"] == null ? "" : xmln.Attributes["Face6ID"].Value;
				this.clerk = xmln.Attributes["clerk"] == null ? "" : xmln.Attributes["clerk"].Value;
				this.PkgNo = xmln.Attributes["PkgNo"] == null ? "" : xmln.Attributes["PkgNo"].Value;
				this.BasicMaterial = xmln.Attributes["BasicMaterial"] == null ? "" : xmln.Attributes["BasicMaterial"].Value;
				this.PartNumber = xmln.Attributes["PartNumber"] == null ? "" : xmln.Attributes["PartNumber"].Value;
				this.DoorDirection = xmln.Attributes["DoorDirection"] == null ? "" : xmln.Attributes["DoorDirection"].Value;
				this.Category = xmln.Attributes["Category"] == null ? "" : xmln.Attributes["Category"].Value;
				this.BasicMaterialCode = xmln.Attributes["BasicMaterialCode"] == null ? "" : xmln.Attributes["BasicMaterialCode"].Value;
				this.BasicMaterialMCode = xmln.Attributes["BasicMaterialMCode"] == null ? "" : xmln.Attributes["BasicMaterialMCode"].Value;
				this.BasicMaterialColorNumber = xmln.Attributes["BasicMaterialColorNumber"] == null ? "" : xmln.Attributes["BasicMaterialColorNumber"].Value;
				this.BasicMaterialColorCode = xmln.Attributes["BasicMaterialColorCode"] == null ? "" : xmln.Attributes["BasicMaterialColorCode"].Value;
				this.EdgeThickCodeNo = xmln.Attributes["EdgeThickCodeNo"] == null ? "" : xmln.Attributes["EdgeThickCodeNo"].Value;
				this.EdgeThickName = xmln.Attributes["EdgeThickName"] == null ? "" : xmln.Attributes["EdgeThickName"].Value;
				this.EdgeThickMaterialCode = xmln.Attributes["EdgeThickMaterialCode"] == null ? "" : xmln.Attributes["EdgeThickMaterialCode"].Value;
				this.EdgeThickWidth = xmln.Attributes["EdgeThickWidth"] == null ? "" : xmln.Attributes["EdgeThickWidth"].Value;
				this.EdgeThickThickness = xmln.Attributes["EdgeThickThickness"] == null ? "" : xmln.Attributes["EdgeThickThickness"].Value;
				this.EdgeThickType = xmln.Attributes["EdgeThickType"] == null ? "" : xmln.Attributes["EdgeThickType"].Value;
				this.EdgeThinCodeNo = xmln.Attributes["EdgeThinCodeNo"] == null ? "" : xmln.Attributes["EdgeThinCodeNo"].Value;
				this.EdgeThinName = xmln.Attributes["EdgeThinName"] == null ? "" : xmln.Attributes["EdgeThinName"].Value;
				this.EdgeThinMaterialCode = xmln.Attributes["EdgeThinMaterialCode"] == null ? "" : xmln.Attributes["EdgeThinMaterialCode"].Value;
				this.EdgeThinWidth = xmln.Attributes["EdgeThinWidth"] == null ? "" : xmln.Attributes["EdgeThinWidth"].Value;
				this.EdgeThinThickness = xmln.Attributes["EdgeThinThickness"] == null ? "" : xmln.Attributes["EdgeThinThickness"].Value;
				this.EdgeThinType = xmln.Attributes["EdgeThinType"] == null ? "" : xmln.Attributes["EdgeThinType"].Value;
				this.EdgeGroupType = xmln.Attributes["EdgeGroupType"] == null ? "" : xmln.Attributes["EdgeGroupType"].Value;
				this.thickLength = xmln.Attributes["thickLength"] == null ? "" : xmln.Attributes["thickLength"].Value;
				this.thinLength = xmln.Attributes["thinLength"] == null ? "" : xmln.Attributes["thinLength"].Value;
				this.customLength = xmln.Attributes["customLength"] == null ? "" : xmln.Attributes["customLength"].Value;
				this.HasHorizontalHole = xmln.Attributes["HasHorizontalHole"] == null ? "" : xmln.Attributes["HasHorizontalHole"].Value;
				this.ActualLength = xmln.Attributes["ActualLength"] == null ? "" : xmln.Attributes["ActualLength"].Value;
				this.ActualWidth = xmln.Attributes["ActualWidth"] == null ? "" : xmln.Attributes["ActualWidth"].Value;
				this.Series = xmln.Attributes["Series"] == null ? "" : xmln.Attributes["Series"].Value;
				this.priceP = xmln.Attributes["priceP"] == null ? "" : xmln.Attributes["priceP"].Value;
				this.CabinetNo = xmln.Attributes["CabinetNo"] == null ? "" : xmln.Attributes["CabinetNo"].Value;
				this.CabinetPanelNo = xmln.Attributes["CabinetPanelNo"] == null ? "" : xmln.Attributes["CabinetPanelNo"].Value;
			}
		}
	}
	public class Line
	{
		public string ClassName {get{ return "Line";}}
		public string LineID {get;set;}
		public string EndX {get;set;}
		public string EndY {get;set;}
		public string Angle {get;set;}
		public Line()
		{
			LineID = "";
			EndX = "";
			EndY = "";
			Angle = "";
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
				this.LineID = xmln.Attributes["LineID"] == null ? "" : xmln.Attributes["LineID"].Value;
				this.EndX = xmln.Attributes["EndX"] == null ? "" : xmln.Attributes["EndX"].Value;
				this.EndY = xmln.Attributes["EndY"] == null ? "" : xmln.Attributes["EndY"].Value;
				this.Angle = xmln.Attributes["Angle"] == null ? "" : xmln.Attributes["Angle"].Value;
			}
		}
	}
	public class Lines
	{
		public string ClassName {get{ return "Lines";}}
		public List<Line> lineList = new List<Line>();
		public Lines()
		{
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
			}
		}
	}
	public class Handle
	{
		public string ClassName {get{ return "Handle";}}
		public string handleDirection {get;set;}
		public string partNumber {get;set;}
		public Handle()
		{
			handleDirection = "";
			partNumber = "";
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
				this.handleDirection = xmln.Attributes["handleDirection"] == null ? "" : xmln.Attributes["handleDirection"].Value;
				this.partNumber = xmln.Attributes["partNumber"] == null ? "" : xmln.Attributes["partNumber"].Value;
			}
		}
	}
	public class Handles
	{
		public string ClassName {get{ return "Handles";}}
		public List<Handle> handleList = new List<Handle>();
		public Handles()
		{
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
			}
		}
	}
	public class Panels
	{
		public string ClassName {get{ return "Panels";}}
		public List<Panel> panelList = new List<Panel>();
		public Panels()
		{
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
			}
		}
	}
	public class Metal
	{
		public string ClassName {get{ return "Metal";}}
		public string Id {get;set;}
		public string Name {get;set;}
		public string PartNumber {get;set;}
		public string length {get;set;}
		public string width {get;set;}
		public string height {get;set;}
		public string Num {get;set;}
		public string Style {get;set;}
		public string Unit {get;set;}
		public Metal()
		{
			Id = "";
			Name = "";
			PartNumber = "";
			length = "";
			width = "";
			height = "";
			Num = "";
			Style = "";
			Unit = "";
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
				this.Id = xmln.Attributes["Id"] == null ? "" : xmln.Attributes["Id"].Value;
				this.Name = xmln.Attributes["Name"] == null ? "" : xmln.Attributes["Name"].Value;
				this.PartNumber = xmln.Attributes["PartNumber"] == null ? "" : xmln.Attributes["PartNumber"].Value;
				this.length = xmln.Attributes["length"] == null ? "" : xmln.Attributes["length"].Value;
				this.width = xmln.Attributes["width"] == null ? "" : xmln.Attributes["width"].Value;
				this.height = xmln.Attributes["height"] == null ? "" : xmln.Attributes["height"].Value;
				this.Num = xmln.Attributes["Num"] == null ? "" : xmln.Attributes["Num"].Value;
				this.Style = xmln.Attributes["Style"] == null ? "" : xmln.Attributes["Style"].Value;
				this.Unit = xmln.Attributes["Unit"] == null ? "" : xmln.Attributes["Unit"].Value;
			}
		}
	}
	public class Metals
	{
		public string ClassName {get{ return "Metals";}}
		public List<Metal> metalList = new List<Metal>();
		public Metals()
		{
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
			}
		}
	}
	public class Values
	{
		public string ClassName {get{ return "Values";}}
		public string W {get;set;}
		public string D {get;set;}
		public string H {get;set;}
		public string PX {get;set;}
		public string PY {get;set;}
		public string PZ {get;set;}
		public string RX {get;set;}
		public string RY {get;set;}
		public string RZ {get;set;}
		public string BLM {get;set;}
		public string BRM {get;set;}
		public string BUM {get;set;}
		public string BDM {get;set;}
		public string BFM {get;set;}
		public string BBM {get;set;}
		public Values()
		{
			W = "";
			D = "";
			H = "";
			PX = "";
			PY = "";
			PZ = "";
			RX = "";
			RY = "";
			RZ = "";
			BLM = "";
			BRM = "";
			BUM = "";
			BDM = "";
			BFM = "";
			BBM = "";
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
				this.W = xmln.Attributes["W"] == null ? "" : xmln.Attributes["W"].Value;
				this.D = xmln.Attributes["D"] == null ? "" : xmln.Attributes["D"].Value;
				this.H = xmln.Attributes["H"] == null ? "" : xmln.Attributes["H"].Value;
				this.PX = xmln.Attributes["PX"] == null ? "" : xmln.Attributes["PX"].Value;
				this.PY = xmln.Attributes["PY"] == null ? "" : xmln.Attributes["PY"].Value;
				this.PZ = xmln.Attributes["PZ"] == null ? "" : xmln.Attributes["PZ"].Value;
				this.RX = xmln.Attributes["RX"] == null ? "" : xmln.Attributes["RX"].Value;
				this.RY = xmln.Attributes["RY"] == null ? "" : xmln.Attributes["RY"].Value;
				this.RZ = xmln.Attributes["RZ"] == null ? "" : xmln.Attributes["RZ"].Value;
				this.BLM = xmln.Attributes["BLM"] == null ? "" : xmln.Attributes["BLM"].Value;
				this.BRM = xmln.Attributes["BRM"] == null ? "" : xmln.Attributes["BRM"].Value;
				this.BUM = xmln.Attributes["BUM"] == null ? "" : xmln.Attributes["BUM"].Value;
				this.BDM = xmln.Attributes["BDM"] == null ? "" : xmln.Attributes["BDM"].Value;
				this.BFM = xmln.Attributes["BFM"] == null ? "" : xmln.Attributes["BFM"].Value;
				this.BBM = xmln.Attributes["BBM"] == null ? "" : xmln.Attributes["BBM"].Value;
			}
		}
	}
	public class Part
	{
		public string ClassName {get{ return "Part";}}
		public string name {get;set;}
		public string subType {get;set;}
		public string type {get;set;}
		public string ID {get;set;}
		public string materialId {get;set;}
		public string imagePath {get;set;}
		public string BasicMaterial {get;set;}
		public string Material {get;set;}
		public List<Values> valuesList = new List<Values>();
		public List<Part> partList = new List<Part>();
		public Part()
		{
			name = "";
			subType = "";
			type = "";
			ID = "";
			materialId = "";
			imagePath = "";
			BasicMaterial = "";
			Material = "";
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
				this.name = xmln.Attributes["name"] == null ? "" : xmln.Attributes["name"].Value;
				this.subType = xmln.Attributes["subType"] == null ? "" : xmln.Attributes["subType"].Value;
				this.type = xmln.Attributes["type"] == null ? "" : xmln.Attributes["type"].Value;
				this.ID = xmln.Attributes["ID"] == null ? "" : xmln.Attributes["ID"].Value;
				this.materialId = xmln.Attributes["materialId"] == null ? "" : xmln.Attributes["materialId"].Value;
				this.imagePath = xmln.Attributes["imagePath"] == null ? "" : xmln.Attributes["imagePath"].Value;
				this.BasicMaterial = xmln.Attributes["BasicMaterial"] == null ? "" : xmln.Attributes["BasicMaterial"].Value;
				this.Material = xmln.Attributes["Material"] == null ? "" : xmln.Attributes["Material"].Value;
			}
		}
	}
	public class Cabinet
	{
		public string ClassName {get{ return "Cabinet";}}
		public string CabinetNo {get;set;}
		public string CabinetPanelNo {get;set;}
		public string PositionNumber {get;set;}
		public string Name {get;set;}
		public string Id {get;set;}
		public string Series {get;set;}
		public string Length {get;set;}
		public string Width {get;set;}
		public string Height {get;set;}
		public string Type {get;set;}
		public string CabinetType {get;set;}
		public string SubType {get;set;}
		public string Material {get;set;}
		public string BasicMaterial {get;set;}
		public string Model {get;set;}
		public string CraftMark {get;set;}
		public string PartNumber {get;set;}
		public string GroupName {get;set;}
		public string RoomName {get;set;}
		public string OrderDate {get;set;}
		public string Designer {get;set;}
		public string CustomId {get;set;}
		public string BatchNo {get;set;}
		public string ThinEdgeValue {get;set;}
		public string ThickEdgeValue {get;set;}
		public string OrderNo {get;set;}
		public string AlongSys {get;set;}
		public string Customer {get;set;}
		public string ShopName {get;set;}
		public string OrderName {get;set;}
		public string CustomAddress {get;set;}
		public string DeliveryDate {get;set;}
		public string Tel {get;set;}
		public string Comment {get;set;}
		public string OrderCount {get;set;}
		public string ItemNo {get;set;}
		public string HouseType {get;set;}
		public string DiyOrderNo {get;set;}
		public List<ProduceValues> producevaluesList = new List<ProduceValues>();
		public List<Panels> panelsList = new List<Panels>();
		public List<Metals> metalsList = new List<Metals>();
		public List<Part> partList = new List<Part>();
		public Cabinet()
		{
			CabinetNo = "";
			CabinetPanelNo = "";
			PositionNumber = "";
			Name = "";
			Id = "";
			Series = "";
			Length = "";
			Width = "";
			Height = "";
			Type = "";
			CabinetType = "";
			SubType = "";
			Material = "";
			BasicMaterial = "";
			Model = "";
			CraftMark = "";
			PartNumber = "";
			GroupName = "";
			RoomName = "";
			OrderDate = "";
			Designer = "";
			CustomId = "";
			BatchNo = "";
			ThinEdgeValue = "";
			ThickEdgeValue = "";
			OrderNo = "";
			AlongSys = "";
			Customer = "";
			ShopName = "";
			OrderName = "";
			CustomAddress = "";
			DeliveryDate = "";
			Tel = "";
			Comment = "";
			OrderCount = "";
			ItemNo = "";
			HouseType = "";
			DiyOrderNo = "";
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
				this.CabinetNo = xmln.Attributes["CabinetNo"] == null ? "" : xmln.Attributes["CabinetNo"].Value;
				this.CabinetPanelNo = xmln.Attributes["CabinetPanelNo"] == null ? "" : xmln.Attributes["CabinetPanelNo"].Value;
				this.PositionNumber = xmln.Attributes["PositionNumber"] == null ? "" : xmln.Attributes["PositionNumber"].Value;
				this.Name = xmln.Attributes["Name"] == null ? "" : xmln.Attributes["Name"].Value;
				this.Id = xmln.Attributes["Id"] == null ? "" : xmln.Attributes["Id"].Value;
				this.Series = xmln.Attributes["Series"] == null ? "" : xmln.Attributes["Series"].Value;
				this.Length = xmln.Attributes["Length"] == null ? "" : xmln.Attributes["Length"].Value;
				this.Width = xmln.Attributes["Width"] == null ? "" : xmln.Attributes["Width"].Value;
				this.Height = xmln.Attributes["Height"] == null ? "" : xmln.Attributes["Height"].Value;
				this.Type = xmln.Attributes["Type"] == null ? "" : xmln.Attributes["Type"].Value;
				this.CabinetType = xmln.Attributes["CabinetType"] == null ? "" : xmln.Attributes["CabinetType"].Value;
				this.SubType = xmln.Attributes["SubType"] == null ? "" : xmln.Attributes["SubType"].Value;
				this.Material = xmln.Attributes["Material"] == null ? "" : xmln.Attributes["Material"].Value;
				this.BasicMaterial = xmln.Attributes["BasicMaterial"] == null ? "" : xmln.Attributes["BasicMaterial"].Value;
				this.Model = xmln.Attributes["Model"] == null ? "" : xmln.Attributes["Model"].Value;
				this.CraftMark = xmln.Attributes["CraftMark"] == null ? "" : xmln.Attributes["CraftMark"].Value;
				this.PartNumber = xmln.Attributes["PartNumber"] == null ? "" : xmln.Attributes["PartNumber"].Value;
				this.GroupName = xmln.Attributes["GroupName"] == null ? "" : xmln.Attributes["GroupName"].Value;
				this.RoomName = xmln.Attributes["RoomName"] == null ? "" : xmln.Attributes["RoomName"].Value;
				this.OrderDate = xmln.Attributes["OrderDate"] == null ? "" : xmln.Attributes["OrderDate"].Value;
				this.Designer = xmln.Attributes["Designer"] == null ? "" : xmln.Attributes["Designer"].Value;
				this.CustomId = xmln.Attributes["CustomId"] == null ? "" : xmln.Attributes["CustomId"].Value;
				this.BatchNo = xmln.Attributes["BatchNo"] == null ? "" : xmln.Attributes["BatchNo"].Value;
				this.ThinEdgeValue = xmln.Attributes["ThinEdgeValue"] == null ? "" : xmln.Attributes["ThinEdgeValue"].Value;
				this.ThickEdgeValue = xmln.Attributes["ThickEdgeValue"] == null ? "" : xmln.Attributes["ThickEdgeValue"].Value;
				this.OrderNo = xmln.Attributes["OrderNo"] == null ? "" : xmln.Attributes["OrderNo"].Value;
				this.AlongSys = xmln.Attributes["AlongSys"] == null ? "" : xmln.Attributes["AlongSys"].Value;
				this.Customer = xmln.Attributes["Customer"] == null ? "" : xmln.Attributes["Customer"].Value;
				this.ShopName = xmln.Attributes["ShopName"] == null ? "" : xmln.Attributes["ShopName"].Value;
				this.OrderName = xmln.Attributes["OrderName"] == null ? "" : xmln.Attributes["OrderName"].Value;
				this.CustomAddress = xmln.Attributes["CustomAddress"] == null ? "" : xmln.Attributes["CustomAddress"].Value;
				this.DeliveryDate = xmln.Attributes["DeliveryDate"] == null ? "" : xmln.Attributes["DeliveryDate"].Value;
				this.Tel = xmln.Attributes["Tel"] == null ? "" : xmln.Attributes["Tel"].Value;
				this.Comment = xmln.Attributes["Comment"] == null ? "" : xmln.Attributes["Comment"].Value;
				this.OrderCount = xmln.Attributes["OrderCount"] == null ? "" : xmln.Attributes["OrderCount"].Value;
				this.ItemNo = xmln.Attributes["ItemNo"] == null ? "" : xmln.Attributes["ItemNo"].Value;
				this.HouseType = xmln.Attributes["HouseType"] == null ? "" : xmln.Attributes["HouseType"].Value;
				this.DiyOrderNo = xmln.Attributes["DiyOrderNo"] == null ? "" : xmln.Attributes["DiyOrderNo"].Value;
			}
		}
	}
	public class baseLine
	{
		public string ClassName {get{ return "baseLine";}}
		public string OrderNo {get;set;}
		public baseLine()
		{
			OrderNo = "";
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
				this.OrderNo = xmln.Attributes["OrderNo"] == null ? "" : xmln.Attributes["OrderNo"].Value;
			}
		}
	}
	public class tables
	{
		public string ClassName {get{ return "tables";}}
		public string OrderNo {get;set;}
		public tables()
		{
			OrderNo = "";
		}

		public void LoadFromXmlNode(XmlNode xmln)
		{
			if (xmln.Name == ClassName)
			{
				this.OrderNo = xmln.Attributes["OrderNo"] == null ? "" : xmln.Attributes["OrderNo"].Value;
			}
		}
	}
}
