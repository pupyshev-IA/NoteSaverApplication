using Abdt.Loyal.NoteSaver.Domain;

namespace Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions
{
    public interface INoteValidator
    {
        /// <summary>
        /// Проверяет правильность введенных данных
        /// </summary>
        /// <returns>Возвращает true если все данные введены верно и false если нет</returns>
        public bool IsValid(Note note);
    }
}
