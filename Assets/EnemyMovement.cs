using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed = 5f;
    private float rotationSpeed = 5f; // Kecepatan rotasi
    private Transform[] waypoints; // Array dari waypoints
    private int currentWaypointIndex = 0; // Index waypoint yang sedang dituju

    // Fungsi untuk mengatur kecepatan musuh
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Fungsi untuk mengatur waypoints yang harus dilalui oleh musuh
    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }

    private void Update()
    {
        if (waypoints != null && currentWaypointIndex < waypoints.Length)
        {
            // Hitung arah menuju waypoint saat ini
            Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;

            // Rotasi musuh ke arah waypoint
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // Menggerakkan musuh menuju waypoint dengan MoveTowards untuk pergerakan yang lebih halus
            Vector3 newPosition = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);
            transform.position = newPosition;

            // Jika musuh sudah mendekati waypoint, pindah ke waypoint berikutnya
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
            {
                currentWaypointIndex++; // Berpindah ke waypoint berikutnya
            }
        }
        else
        {
            // Jika sudah mencapai waypoint terakhir, musuh bisa dihancurkan atau logika lain bisa diterapkan
            if (currentWaypointIndex >= waypoints.Length)
            {
                Destroy(gameObject); // Menghancurkan musuh setelah mencapai waypoint terakhir
            }
        }
    }
}
