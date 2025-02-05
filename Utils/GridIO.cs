//============================================================================
// GridIO.cs
//============================================================================
using Godot;
using System;

public partial class GridIO : GridContainer
{
	enum CellType{
		Label,
		TextBox,
	}
	CellType[,] cellType;

	int nRow;      // number of rows
	int nCol;      // number of columns

	Label[,] label;
	string[,] fString;  // format strings (for numeric data)
	string[,] decString; // decimal strings
	string[,] sfxString; // suffix string

	bool sized;    // whether size has been set
	bool initialized; // whether initialized

	//------------------------------------------------------------------------
	// _Ready
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		//GD.Print("GridIO_Ready");

		sized = false;
		initialized = false;


	}

	//------------------------------------------------------------------------
	// SetSize: Set the size of the grid
	//------------------------------------------------------------------------
	public void SetSize(int nr, int nc)
	{
		if(sized){
			GD.PrintErr("GridIO: ERROR. Size has already been set.");
			return;
		}

		if(nr < 1){
			GD.PrintErr("GridIO: ERROR. Need at least one row.");
			return;
		}

		if(nc < 1){
			GD.PrintErr("GridIO: ERROR. Need at least one column.");
			return;
		}

		nRow = nr;   nCol = nc;
		cellType = new CellType[nRow, nCol];
		label = new Label[nRow, nCol];
		fString = new string[nRow, nCol];
		decString = new string[nRow, nCol];
		sfxString = new string[nRow, nCol];

		for(int i=0; i<nRow; ++i){
			for(int j=0; j<nCol; ++j){
				cellType[i,j] = CellType.Label;
				label[i,j] = new Label();
				label[i,j].Text = "Default";
				fString[i,j] = "0.00";
				decString[i,j] = "0.00";
				sfxString[i,j] = "";
			}
		}

		sized = true;
	}

	//------------------------------------------------------------------------
	// InitGridCells: Initialize cells in the grid.... just default now
	//------------------------------------------------------------------------
	public void InitGridCells()
	{
		if(!sized){
			GD.PrintErr("GridIO::InitCells: Not sized yet.");
			return;
		}

		this.Columns = nCol;
		for(int i=0; i<nRow; ++i){
			for(int j=0; j<nCol; ++j){
				if(cellType[i,j] == CellType.Label){
					AddChild(label[i,j]);
				}
				else{
					GD.PrintErr("GridIO:InitGridCells: unknown cell type");
					return;
				}
			}
		}

		initialized = true;
	}

	//------------------------------------------------------------------------
	// SetText:
	//------------------------------------------------------------------------
	public void SetText(int ii, int jj, string txt)
	{
		if(!initialized){
			GD.PrintErr("GridIO::SetText:Error: Not initialized");
			return;
		}

		if(ii < 0 || ii >= nRow){
			string err_txt = "GridIO::SetText:Error row index " + ii + 
				" out of range.";
			GD.PrintErr(err_txt);
			return;
		}

		if(jj < 0 || jj >= nCol){
			string err_txt = "GridIO::SetText:Error column index " + jj + 
				" out of range.";
			GD.PrintErr(err_txt);
			return;
		}

		if(cellType[ii,jj] == CellType.Label){
			label[ii,jj].Text = txt;
		}
		else{
			GD.PrintErr("GridIO:SetText:Error unknown cell type");
					return;
		}
	}

	//------------------------------------------------------------------------
	// SetNumeric:
	//------------------------------------------------------------------------
	public void SetNumeric(int ii, int jj, float val)
	{
		if(!initialized){
			GD.PrintErr("GridIO::SetNumeric:Error: Not initialized");
			return;
		}

		if(ii < 0 || ii >= nRow){
			string err_txt = "GridIO::SetNumeric:Error: row index " + ii + 
				" out of range.";
			GD.PrintErr(err_txt);
			return;
		}

		if(jj < 0 || jj >= nCol){
			string err_txt = "GridIO::SetNumeric:Error column index " + jj + 
				" out of range.";
			GD.PrintErr(err_txt);
			return;
		}

		if(cellType[ii,jj] == CellType.Label){
			label[ii,jj].Text = val.ToString(fString[ii,jj]);
		}
		else{
			GD.PrintErr("GridIO:SetNumeric:Error unknown cell type");
					return;
		}
	}

	//------------------------------------------------------------------------
	// SetDigitsAfterDecimal:
	//------------------------------------------------------------------------
	public void SetDigitsAfterDecimal(int ii, int jj, int nd)
	{
		if(!initialized){
			GD.PrintErr(
				"GridIO::SetDigitsAfterDecimal:Error: Not initialized");
			return;
		}

		if(ii < 0 || ii >= nRow){
			string err_txt = 
				"GridIO::SetDigitsAfterDecimal:Error: row index " + ii + 
				" out of range.";
			GD.PrintErr(err_txt);
			return;
		}

		if(jj < 0 || jj >= nCol){
			string err_txt = 
				"GridIO::SetDigitsAfterDecimal:Error column index " + jj + 
				" out of range.";
			GD.PrintErr(err_txt);
			return;
		}

		if(nd < 0)
			return;

		if(nd > 10)
			nd = 10;

		decString[ii,jj] = "0.";
		for(int i=0; i<nd; ++i){
			decString[ii,jj] += "0";
		}

		fString[ii,jj] = decString[ii,jj] + sfxString[ii,jj];
	}

	//------------------------------------------------------------------------
	// SetYellow: Sets the color of text in specified cell to yellow
	//------------------------------------------------------------------------
	public void SetYellow(int ii, int jj)
	{
		if(!initialized){
			GD.PrintErr(
				"GridIO::SetYellow:Error: Not initialized");
			return;
		}

		if(ii < 0 || ii >= nRow){
			string err_txt = 
				"GridIO::SetYellow:Error: row index " + ii + 
				" out of range.";
			GD.PrintErr(err_txt);
			return;
		}

		if(jj < 0 || jj >= nCol){
			string err_txt = 
				"GridIO::SetYellow:Error column index " + jj + 
				" out of range.";
			GD.PrintErr(err_txt);
			return;
		}

		label[ii,jj].Set("theme_override_colors/font_color",new Color(1,1,0));
	}

	//------------------------------------------------------------------------
	// SetMagenta: Sets the color of text in specified cell to magenta
	//------------------------------------------------------------------------
	public void SetMagenta(int ii, int jj)
	{
		if(!initialized){
			GD.PrintErr(
				"GridIO::SetYellow:Error: Not initialized");
			return;
		}

		if(ii < 0 || ii >= nRow){
			string err_txt = 
				"GridIO::SetYellow:Error: row index " + ii + 
				" out of range.";
			GD.PrintErr(err_txt);
			return;
		}

		if(jj < 0 || jj >= nCol){
			string err_txt = 
				"GridIO::SetYellow:Error column index " + jj + 
				" out of range.";
			GD.PrintErr(err_txt);
			return;
		}

		label[ii,jj].Set("theme_override_colors/font_color",new Color(1,0,1));
	}

	//------------------------------------------------------------------------
	// SetCyan: Sets the color of text in specified cell to Cyan
	//------------------------------------------------------------------------
	public void SetCyan(int ii, int jj)
	{
		if(!initialized){
			GD.PrintErr(
				"GridIO::SetYellow:Error: Not initialized");
			return;
		}

		if(ii < 0 || ii >= nRow){
			string err_txt = 
				"GridIO::SetYellow:Error: row index " + ii + 
				" out of range.";
			GD.PrintErr(err_txt);
			return;
		}

		if(jj < 0 || jj >= nCol){
			string err_txt = 
				"GridIO::SetYellow:Error column index " + jj + 
				" out of range.";
			GD.PrintErr(err_txt);
			return;
		}

		label[ii,jj].Set("theme_override_colors/font_color",new Color(0,1,1));
	}

	//public override void _Process(double delta)
	//{
	//}
}
