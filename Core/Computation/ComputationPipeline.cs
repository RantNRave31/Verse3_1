using Core.Elements;
using Core.Nodes;
using System;

namespace Core
{
    public class ComputationPipeline
    {
        private static ComputationPipeline instance = new ComputationPipeline();
        public static ComputationPipeline Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ComputationPipeline();
                }
                return ComputationPipeline.instance;
            }
            protected set
            {
                instance = value;
            }
        }
        internal IComputable _current;
        public IComputable Current => _current;
        private ComputationPipeline()
        {
            this._current = default;
        }

        public static int Compute(IComputable sender = null)
        {
            int count = 0;
            try
            {
                if (sender != null)
                {
                    count = ComputeComputable(sender);
                }
                else if (ComputationPipeline.Instance._current != null)
                {
                    count = ComputeComputable(ComputationPipeline.Instance._current);
                }
            }
            catch (Exception e)
            {
                //TODO: Log to console
                CoreConsole.Log(e);
            }
            return count;
        }





        public static int ComputeComputable(IComputable computable, bool recursive = true/*, bool upstream = false*//*, bool render = true*/)
        {
            if (computable == null) return -1;
            //TODO: PARALLEL COMPUTATION
            if (computable.ComputableElementState == ComputableElementState.Computing) return -1;
            else computable.ComputableElementState = ComputableElementState.Computing;
            int count = 0;
            try
            {
                bool computeSuccess = true;
                if (computable != null)
                {

                    ComputationPipeline.Instance._current = computable;
                    //if (computable.ComputationPipelineInfo.IOManager.EventInputNodes != null &&
                    //    computable.ComputationPipelineInfo.IOManager.EventInputNodes.Count > 0)
                    //{
                    //    //TODO: Log to console
                    //    //Computables with EventUS will only compute when their EventUS computables trigger the event they are listening to
                    //    computable.CollectData();
                    //}
                    //else
                    //{
                    //ComputeForDataStructureFlag
                    //TODO: Create a task to compute and store the task in the computable as part of it's state
                    //https://medium.com/@alex.puiu/parallel-foreach-async-in-c-36756f8ebe62
                    if (computable.CollectData())
                    {
                        computeSuccess = false;
                        computable.ComputationPipelineInfo.ComputableElementState = ComputableElementState.Failed;
                        computable.OnLog_Internal(new EventArgData(new DataStructure<string>("CollectData failed - Check Inputs.")));
                    }
                    else
                    {
                        computable.ComputationPipelineInfo.ComputableElementState = ComputableElementState.Computing;
                        computable.Compute();
                        computable.ComputationPipelineInfo.ComputableElementState = ComputableElementState.Computed;
                        computable.DeliverData();

                        count++;
                        if (recursive)
                        {
                            //if (!upstream)
                            //{
                            if (computable.ComputationPipelineInfo.DataDS != null && computable.ComputationPipelineInfo.DataDS.Count > 0)
                            {
                                foreach (IComputable compDS in computable.ComputationPipelineInfo.DataDS)
                                {
                                    //TODO: Log to console
                                    computeSuccess = computeSuccess && (ComputeComputable(compDS) > 0);
                                }
                            }
                            //}
                            //else
                            //{
                            //if (computable.ComputationPipelineInfo.DataUS != null && computable.ComputationPipelineInfo.DataUS.Count > 0)
                            //{
                            //    foreach (IComputable compUS in computable.ComputationPipelineInfo.DataUS)
                            //    {
                            //        //TODO: Log to console
                            //        computeSuccess = computeSuccess && (ComputeComputable(compUS) > 0);
                            //    }
                            //}
                            //}
                        }
                    }
                    
                    //}
                }
                if (computeSuccess) return count;
                else return -1;
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex, computable);
                computable.ComputableElementState = ComputableElementState.Failed;
            }
            finally
            {
                //TODO: Don't render if running Headlessly (ENV VARIABLE)
                computable.ComputableElementState = ComputableElementState.Computed;
                if (computable is IRenderable)
                {
                    IRenderable r = computable as IRenderable;
                    //RenderPipeline.RenderRenderable(r);
                    RenderingCore.Render(r);
                }
            }
            return count;
        }
    }
}
