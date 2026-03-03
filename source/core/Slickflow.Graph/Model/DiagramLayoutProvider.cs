namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Provides the diagram layout implementation used when serializing process XML.
    /// Default is SimpleBpmnDiagramLayout. Set Current to EnterpriseBpmnDiagramLayout when using Enterprise.
    /// </summary>
    public static class DiagramLayoutProvider
    {
        private static IBpmnDiagramLayout _current = new SimpleBpmnDiagramLayout();

        /// <summary>
        /// Current layout implementation. Default is simple two-point edges.
        /// </summary>
        public static IBpmnDiagramLayout Current
        {
            get => _current;
            set => _current = value ?? new SimpleBpmnDiagramLayout();
        }
    }
}
