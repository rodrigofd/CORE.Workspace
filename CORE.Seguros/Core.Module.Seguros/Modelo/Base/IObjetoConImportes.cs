namespace FDIT.Core.Seguros
{
    public interface IObjetoConImportes
    {
        object Evaluate(string expression);
        void RecalcularImportes(DocumentoImporteBase excluir);
    }
}