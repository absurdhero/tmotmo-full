// The iPhone position sensor is pretty jittery. This class calms it down 
public class DeShaker {
  int bufferSize;

  float[] rawX;
  float[] rawY;
  int counter;

  public DeShaker(int bufferSize = 4) {
    this.bufferSize = bufferSize;
    rawX = new float[bufferSize];
    rawY = new float[bufferSize];
  }
  
  public void shakyPoint(float shakyX, float shakyY) {
    if (counter == 0) {
      for (int i=0; i<bufferSize; i++) { 
        rawX[i] = shakyX;
        rawY[i] = shakyY;
      }
    }
    counter++;
    int index = counter % bufferSize;
    rawX[index] = shakyX;
    rawY[index] = shakyY;
  }
  
  public float firmX() {
    return average(rawX);
  }
  
  public float firmY() {
    return average(rawY);
  }
  
  private float average(float[] shaky) {
    float sum = 0;
    foreach(float s in shaky) {
      sum += s;
    }
    return sum/bufferSize;
  }
}
