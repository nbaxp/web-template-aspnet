using System.ComponentModel.DataAnnotations;

namespace WebTemplate.Models;

/// <summary>
/// 1.是否可空通过IsRequied属性获取
/// 2.数据模型可以使用DateTime，但视图模型一定要使用带时区的DateTimeOffset
/// </summary>
[Display(Description = "Class 转 Form")]
public class Class2FormModel
{
    public bool Bool_Input_Checkbox { get; set; }
    public int Int_Input_Number { get; set; }
    public long Long_Input_Text { get; set; }
    public DateTimeOffset DateTimeOffset_Input_DateTimeLocal { get; set; }
    [DataType(DataType.Date)]
    public DateTimeOffset DateTimeOffset_Date_InputDate { get; set; }
    [DataType(DataType.Time)]
    public DateTimeOffset DateTimeOffset_Date_InputTime { get; set; }
    public string string2InputText { get; set; } = null!;
}
