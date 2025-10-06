using Microsoft.Extensions.VectorData;

namespace VectorDataAI.Models
{
    internal class CloudService
    {
        public CloudService()
        {
            // Constructor logic here
        }

        // Add methods and properties as needed
        [VectorStoreKey]
        public int Key { get; set; }

        [VectorStoreData]
        public string Name { get; set; }

        [VectorStoreData]
        public string Description { get; set; }

        [VectorStoreVector(Dimensions: 384, DistanceFunction = DistanceFunction.CosineSimilarity)]
        public ReadOnlyMemory<float> Vector { get; set; }
    }
}