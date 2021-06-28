using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using System;
using System.Collections.Generic;
using System.Text;

namespace project_pyro_rewrite.Effects
{
    public class TestingPostProcessor : PostProcessor
    {
        public TestingPostProcessor(int executionOrder, Effect effect = null) : base(executionOrder, effect)
        {
        
        }

        private Vector2 strongPoint;
        private float totalTimeInMs = 1000f;
        private float timerMs = 0f;
        private float frequency = 0.01f;
        private float amplitude = 0.005f;

        EffectParameter strongPointParam;
        EffectParameter maxEffectiveDistanceParam;
        EffectParameter totalTimeInMsParam;
        EffectParameter timerMsParam;
        EffectParameter frequencyParam;
        EffectParameter amplitudeParam;

        public Vector2 StrongPoint
        {
            get { return strongPoint; }
            set
            {

                strongPoint = value;
                strongPointParam?.SetValue(value);
            }
        }

        public float TotalTimeInMs
        {
            get { return totalTimeInMs; }
            set
            {
                float val = value;
                if (val < 0f) val = 0f;
                totalTimeInMs = val;
                totalTimeInMsParam?.SetValue(val);
            }
        }

        public float TimerMs
        {
            set
            {
                float val = value;
                timerMs = val;
                timerMsParam?.SetValue(val);
            }
        }

        public override void OnAddedToScene(Scene scene)
        {
            base.OnAddedToScene(scene);

            strongPointParam = Effect.Parameters["strongPoint"];
            totalTimeInMsParam = Effect.Parameters["totalTimeMs"];
            timerMsParam = Effect.Parameters["timerMs"];
            frequencyParam = Effect.Parameters["frequency"];
            amplitudeParam = Effect.Parameters["amplitude"];
            Enabled = false;
        }

        public void Play(Vector2 strongPoint, float totalTimeInMs)
        {
            StrongPoint = strongPoint;
            TotalTimeInMs = totalTimeInMs;
            timerMs = 0f;
            frequencyParam?.SetValue(frequency);
            amplitudeParam?.SetValue(amplitude);
            Enabled = true;
        }

        public void Update()
        {
            if (timerMs > totalTimeInMs)
            {
                Enabled = false;
            }
            else
            {
                TimerMs = timerMs + (Time.DeltaTime * 1000f);
            }
        }
    }
}
