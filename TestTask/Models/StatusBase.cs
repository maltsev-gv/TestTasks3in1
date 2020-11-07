using TestTask.Common;

namespace TestTask.Models
{
    public class StatusBase : ObservableObject
    {
        public string Status
        {
            get => GetVal<string>();
            set => SetVal(value);
        }

        public bool? IsSuccess
        {
            get => GetVal<bool?>();
            set => SetVal(value);
        }
        public int? ThreadId
        {
            get => GetVal<int?>();
            protected set => SetVal(value);
        }

        public int? RequestNumber
        {
            get => GetVal<int?>();
            protected set => SetVal(value);
        }

    }
}
