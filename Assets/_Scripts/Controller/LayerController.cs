using System;

namespace _Scripts.Controller {
    public static class LayerController {
        public static int GetLayer(int layerValue) {
            int layer = 0;
            int counter = 1;
            do {
                counter = 1 << ++layer;
                Console.WriteLine(counter);
            } while (counter < layerValue);

            return layer;
        }
    }
}