using System.Collections.Generic;
using UnityEngine;

namespace JKQScreenshotsToolMod
{
  public struct ScreenshotRangeValues
  {
    private Vector2 xRange;
    private float xIncrement;

    private Vector2 yRange;
    private float yIncrement;

    private bool calculatedScreenshots;

    public List<Vector3> allScreenshotPositions { get; private set; }
    public Vector2 amountOfScreenshotsPerAxis { get; private set; }
    public uint amountOfScreenshots { get; private set; }


    public ScreenshotRangeValues(Vector2 xRange, Vector2 yRange, float xIncrement, float yIncrement)
    {
      this.xRange = xRange;
      this.yRange = yRange;
      this.xIncrement = xIncrement;
      this.yIncrement = yIncrement;

      calculatedScreenshots = false;

      allScreenshotPositions = new List<Vector3>();
      amountOfScreenshotsPerAxis = new Vector2();
      amountOfScreenshots = 0u;
    }

    public Vector2 GetXRange() => xRange;
    public void SetXRange(Vector2 xRange)
    {
      this.xRange = xRange;
      calculatedScreenshots = false;
    }

    public Vector2 GetYRange() => yRange;
    public void SetYRange(Vector2 yRange)
    {
      this.yRange = yRange;
      calculatedScreenshots = false;
    }

    public float GetXIncrement() => xIncrement;
    public void SetXIncrement(float xIncrement)
    {
      this.xIncrement = xIncrement;
      calculatedScreenshots = false;
    }

    public float GetYIncrement() => yIncrement;
    public void SetYIncrement(float yIncrement)
    {
      this.yIncrement = yIncrement;
      calculatedScreenshots = false;
    }

    public bool CanTakeScreenshots()
    {
      // Checks if the ranges introduced make sense
      bool xRangeValid = xRange.x <= xRange.y;
      bool xIncrementValid = xIncrement <= (xRange.y - xRange.x);

      bool yRangeValid = yRange.x <= yRange.y;
      bool yIncrementValid = yIncrement <= (yRange.y - yRange.x);

      return xRangeValid && xIncrementValid && yRangeValid && yIncrementValid;
    }
    public bool TryCalculateScreenshotPositions()
    {
      if (!CanTakeScreenshots()) return false;

      if (!calculatedScreenshots)
      {
        CalculateScreenshotsPositions();
      }
      return true;
    }

    private void CalculateScreenshotsPositions()
    {
      // Gets all the positions to take screenshots of
      allScreenshotPositions.Clear();

      // Tracks how many screenshots the tool will take (columns * rows)
      uint screenshotRows = 0u;
      uint screenshotColumns = 0u;

      // The amount of rows is calculated within the first Columns loop
      bool completedFirstLoopX = false;

      // Break condition is inside each loop
      // Calculates columns first, rows second in a sequence
      // Continually increment Y until it reaches or surpases the positive Y Limit
      for (float currentIncrementY = 0f; ; currentIncrementY += yIncrement)
      {
        screenshotColumns++;

        float currentPosY = yRange.x + currentIncrementY;
        bool reachedLimitY = false;

        // If the current position exceed the positive Y Limit, clamps the value to the limit
        if (currentPosY + yIncrement > yRange.y)
        {
          currentPosY = yRange.y;
          reachedLimitY = true;
        }

        // Soft-lock prevention
        if (yIncrement <= 0f)
        {
          currentPosY = yRange.y;
          reachedLimitY = true;
        }

        // Continually increment X until it reaches or surpases the positive X Limit
        for (float currentIncrementX = 0f; ; currentIncrementX += xIncrement)
        {
          // Only increase the rows value within the first Columns loop
          if (!completedFirstLoopX) screenshotRows++;

          float currentPosX = xRange.x + currentIncrementX;
          bool reachedLimitX = false;

          // If the current position exceed the positive X Limit, clamps the value to the limit
          if (currentPosX + xIncrement > xRange.y)
          {
            currentPosX = xRange.y;
            reachedLimitX = true;
          }

          // Soft-lock prevention
          if (xIncrement <= 0f)
          {
            currentPosX = xRange.y;
            reachedLimitX = true;
          }

          allScreenshotPositions.Add(new Vector3(currentPosX, currentPosY, 0f));

          if (reachedLimitX) break;
        }

        // Prevents increasing the rows number after the first Columns loop
        completedFirstLoopX = true;

        if (reachedLimitY) break;
      }

      amountOfScreenshotsPerAxis = new Vector2(screenshotRows, screenshotColumns);
      amountOfScreenshots = screenshotColumns * screenshotRows;
      calculatedScreenshots = true;
    }
  }
}