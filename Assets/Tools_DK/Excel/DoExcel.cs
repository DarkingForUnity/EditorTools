//=====================================================
// - FileName:      DoExcel.cs
// - CreateTime:    #CreateTime#
// - Description:   脚本描述 
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using Excel;

public class DoExcel
{

    public static DataSet ReadExcel(string path)
    {
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        // CreateOpenXmlReader用于读取Excel2007版本及以上的文件
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();
        excelReader.Close();
        return result;
    }
    public static List<DepenceTableData> Load(string path)
    {
        List<DepenceTableData> _data = new List<DepenceTableData>();
        DataSet resultds = ReadExcel(path);
        int column = resultds.Tables[0].Columns.Count;
        int row = resultds.Tables[0].Rows.Count;
        Debug.LogWarning(column + "  " + row);
        for (int i = 0; i < row; i++)
        {
            DepenceTableData temp_data;
            temp_data.instruct = resultds.Tables[0].Rows[i][0].ToString();
            temp_data.word = resultds.Tables[0].Rows[i][1].ToString();
            Debug.Log(temp_data.instruct + "  " + temp_data.word);
            _data.Add(temp_data);
        }
        return _data;
    }
}

public struct DepenceTableData
{
    public string word;
    public string instruct;
}
