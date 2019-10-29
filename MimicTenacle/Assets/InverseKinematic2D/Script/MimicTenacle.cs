using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicTenacle : MonoBehaviour
{
    [SerializeField, Range(0, 10)]
    int hand_nums;

    [SerializeField, Range(0, 10)]
    float max_distance;

    [SerializeField]
    Transform landingHolder;

    SpriteRenderer[] landingPoints;

    Hand[] hands;

    private List<Transform> target_list;

    private void Start() {
        target_list = new List<Transform>();
        landingPoints = landingHolder.GetComponentsInChildren<SpriteRenderer>();
        hands = new Hand[hand_nums];

        for (int i = 0; i < hand_nums; i++) {
            hands[i] = new Hand();
            hands[i].pos = transform.position;
            //hands[i];
        }
    }

    private void Update() {
        target_list.Clear();

        target_list = GetViableLandingPoints(target_list);

        UpdateHandPos();

        Hand freehand = FindFreeHand();
        //if (freehand.pos != Vector3.zero && target_list)
        //{

        //}

    }

    private void UpdateHandPos() {
        for (int i = 0; i < hand_nums; i++)
        {
            float distance = Vector3.Distance(hands[i].pos, transform.position);
            Vector3 direction = (hands[i].pos - transform.position).normalized;

            if (distance > max_distance) {
                if (hands[i].landPoint != null) {
                    hands[i].landPoint = null;
                }

                hands[i].pos = (direction * max_distance)　+ transform.position;
            }
        }
    }

    private List<Transform> GetViableLandingPoints(List<Transform> transform_list) {

        int landingPointCount = landingPoints.Length;
        for (int i = 0; i < landingPointCount; i++) {

            float distance = Vector3.Distance(landingPoints[i].transform.position, transform.position);
            if (distance < max_distance) {
                landingPoints[i].color = Color.red;
                transform_list.Add(landingPoints[i].transform);
            } else
            {
                landingPoints[i].color = Color.white;
            }
        }

        return transform_list;
    }

    private Hand FindFreeHand() {

        for (int i = 0; i < hand_nums; i++) {
            if (hands[i].landPoint != null) {
                return hands[i];
            }
        }

        return default(Hand);
    }

    private void OnDrawGizmos()
    {
        try
        {
            if (hands == null) return;

            for (int i = 0; i < hand_nums; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(hands[i].pos, 0.1f);
            }
        }
        catch {
            hands = null;
        }

    }

    public struct Hand {
        public Vector3 pos;
        public Transform landPoint;
    }
}
