using System.ComponentModel.DataAnnotations;

namespace WebTemplate.Web.Models
{
    /// <summary>
    /// https://docs.microsoft.com/zh-cn/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-6.0
    /// https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.ViewFeatures/src/TemplateRenderer.cs
    /// https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.ViewFeatures/src/DefaultEditorTemplates.cs
    /// </summary>
    public class TypeViewModel
    {
        public sbyte InputNumber_sbyte { get; set; }
        public byte InputNumber_byte { get; set; }
        public short InputNumber_short { get; set; }
        public ushort InputNumber_ushort { get; set; }
        public int InputNumber_int { get; set; }
        public uint InputNumber_uint { get; set; }
        public long InputNumber_long { get; set; }
        public ulong InputNumber_ulong { get; set; }

        //public BigInteger InputNumber_BigInteger { get; set; }//不支持
        //public nint InputNumber_nint { get; set; }//不支持
        //public nuint InputNumber_nuint { get; set; }//不支持
        public float InputText_float { get; set; }//服务端验证

        public double InputText_double { get; set; }//服务端验证
        public decimal InputText_decimal { get; set; }//服务端验证

        //public Half Input_Text_half_float16 { get; set;}//不支持
        public char InputText_char { get; set; }//服务端验证

        public bool InputCheckbox_bool { get; set; }
        public string? InputText_string { get; set; }

        [DataType(DataType.Password)]
        public string? InputPassword_string_DataTypePassword { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Textarea_string_DataTypeMultilineText { get; set; }

        [DataType(DataType.Html)]
        public string? InputText_string_DataTypeHtml { get; set; }

        [DataType(DataType.CreditCard)]
        public string? InputText_string_DataTypeCreditCard { get; set; }

        [DataType(DataType.Currency)]
        public string? InputText_string_DataTypeCurrency { get; set; }

        [DataType(DataType.Duration)]
        public string? InputText_string_DataTypeDuration { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? InputEmail_string_DataTypeEmailAddress { get; set; }

        [DataType(DataType.ImageUrl)]
        public string? InputText_string_DataTypeImageUrl { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? InputTel_string_DataTypePhoneNumber { get; set; }

        [DataType(DataType.PostalCode)]
        public string? InputText_string_DataTypePostalCode { get; set; }

        [DataType(DataType.Url)]
        public string? InputUrl_string_DataTypeUrl { get; set; }

        /// <summary>
        /// https://docs.microsoft.com/zh-cn/powerapps/developer/data-platform/file-attributes
        /// </summary>
        [DataType(DataType.Upload)]
        public IFormFile? InputText_string_DataTypeUpload { get; set; }

        public Guid Input_Text_guid { get; set; }//服务端验证
        public DateTime InputDatetime_Local_DateTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime InputDate_Date { get; set; }

        [DataType(DataType.Time)]
        public DateTime InputTime_Time { get; set; }

        //public DateOnly Input_Datetime_Local_DateOnly { get; set; }//不支持
        //public TimeOnly Input_Datetime_Local_TimeOnly { get; set; }//不支持
        public DateTimeOffset InputText_DateTimeOffset { get; set; }

        public CustomEnum InputText_CustomEnum { get; set; }

        //[DataType("DropDownList")]
        public CustomEnum Select_CustomEnum_HtmlHelperDropDownList { get; set; }

        //[DataType("ListBox")]
        public CustomEnum SelectMultiple_CustomEnum_HtmlHelperListBoxFor { get; set; }

        public CustomEnum InputRadio_string_HtmlRadioButtonFor { get; set; }
    }

    public enum CustomEnum
    {
        [Display(Name = "选项1")]
        Option1 = 10,

        [Display(Name = "选项1")]
        Option2 = 20,

        [Display(Name = "选项3")]
        Option3 = 30,
    }
}
