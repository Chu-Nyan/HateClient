using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace SAB.Cutscene
{
    [CustomTimelineEditor(typeof(EventMarker))]
    public sealed class EventMarkerMarkerEditor : MarkerEditor
    {
        private const float _rangeHeight = 6f;

        public override void DrawOverlay(IMarker marker, MarkerUIStates uiState, MarkerOverlayRegion region)
        {
            var eventMarker = (EventMarker)marker;
            if (eventMarker.Time <= 0f)
                return;

            double startTime = marker.time;
            double endTime = startTime + eventMarker.Time;
            double visibleDuration = region.endTime - region.startTime;

            if (visibleDuration <= 0d || endTime < region.startTime || startTime > region.endTime)
                return;

            Rect timelineRect = region.timelineRegion;
            float pixelsPerSecond = timelineRect.width / (float)visibleDuration;

            float startX = timelineRect.xMin + (float)(startTime - region.startTime) * pixelsPerSecond;
            float endX = timelineRect.xMin + (float)(endTime - region.startTime) * pixelsPerSecond;

            startX = Mathf.Clamp(startX, timelineRect.xMin, timelineRect.xMax);
            endX = Mathf.Clamp(endX, timelineRect.xMin, timelineRect.xMax);

            if (endX <= startX)
                return;

            Rect rangeRect = new Rect(startX, region.markerRegion.center.y - _rangeHeight * 0.5f, endX - startX, _rangeHeight);

            EditorGUI.DrawRect(rangeRect, new Color(0.2f, 0.7f, 1f, 0.25f));
        }
    }

}
