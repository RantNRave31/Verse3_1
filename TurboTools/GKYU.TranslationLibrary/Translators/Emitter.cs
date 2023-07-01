using GKYU.CoreLibrary.Attributes;

namespace GKYU.TranslationLibrary.Translators
{
    public abstract class Emitter<INPUT_TYPE>
        : IWriteObjects<INPUT_TYPE>
    {
        public Parameters Parameters { get; set; }
        public Metrics Metrics { get; set; }
        public virtual bool CanWrite { get; set; }
        public abstract int Write<INPUT_TYPE>(INPUT_TYPE value);
    }
}
