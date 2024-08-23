using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Trash
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField]
        private Grid _Grid;
        [SerializeField]
        private Transform _GridContainer;
        [SerializeField]
        private GameObject _GridPrefab;

        private LevelData _levelData = null;
        private GameObject[] _gridObjects = null;
        private int IDLevel = 1;

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.Minus))
            {
                IDLevel = Mathf.Max(1, IDLevel - 1);
                Debug.Log("Minus Load Level ID: " + IDLevel);
                _levelData = LoadFromFirebase(IDLevel);
                SetupGrid();
            }
            else if (Input.GetKeyUp(KeyCode.Equals))
            {
                IDLevel += 1;
                Debug.Log(" Equals Load Level ID: " + IDLevel);
                _levelData = LoadFromFirebase(IDLevel);
                SetupGrid();
            }

        }

        private async void Start()
        {
            await FirebaseManager.Instance.ConnectToFirebase();
            //await Task.Delay(1000);
            //_levelData = LoadFromFirebase(1);

            //SetupGrid();
        }

        private void SetupGrid()
        {
            _GridContainer.DestroyChildrenImmediate();
            _GridContainer.localPosition = Vector3.zero;

            int maxZ = -100;
            int minZ = 100;

            int maxX = -100;
            int minX = 100;

            for (int i = 0; i < _levelData.Grid.GridHexagonDatas.Length; i++)
            {
                GridHexagonData gridHexagon = _levelData.Grid.GridHexagonDatas[i];
                int col = gridHexagon.Row;
                int row = gridHexagon.Column;

                if (row > maxZ)
                {
                    maxZ = row;
                }


                if (gridHexagon.Column < minZ) { minZ = row; }
                if (col > maxX) { maxX = col; }
                if (col < minX) { minX = col; }

                InitGrid(gridHexagon);
            }

            Debug.Log($"maxZ: {maxZ}, minZ: {minZ}, maxX: {maxX}, minX: {minX}");

            Vector3 topLeft = _Grid.CellToWorld(new Vector3Int(minX, maxZ, 0));
            Debug.Log("topLeft: " + _Grid.WorldToCell(topLeft) + " " + topLeft.ToString());
            Vector3 bottomLeft = _Grid.CellToWorld(new Vector3Int(minX, minZ, 0));
            Debug.Log("bottomLeft: " + _Grid.WorldToCell(bottomLeft) + " " + bottomLeft.ToString());
            Vector3 bottomRight = _Grid.CellToWorld(new Vector3Int(maxX, minZ, 0));
            Debug.Log("bottomRight: " + _Grid.WorldToCell(bottomRight) + " " + bottomRight.ToString());

            Debug.Log(_Grid.transform.position);
            float xCenter = GetXCenterPosition(bottomRight, bottomLeft);
            Debug.Log("xCenter: " + xCenter);
            float zCenter = GetZCenterPosition(topLeft, bottomLeft);
            Debug.Log("zCenter: " + zCenter);

            Debug.DrawLine(_Grid.transform.position, new Vector3(xCenter, 0, zCenter), Color.red, 5f);

            //Vector3 offset = new Vector3(xCenter, 0, zCenter) - _Grid.transform.position.With(y: 0);
            //_GridContainer.transform.localPosition -= offset;

            Vector3 offset = new Vector3(xCenter, 0, zCenter);
            _GridContainer.transform.localPosition -= offset;
        }        

        private LevelData LoadFromFirebase(int IDLevel)
        {
            LevelData levelData = FirebaseManager.Instance.GetRemoteLevelData(IDLevel);
            levelData.DebugLogObject();

            return levelData;
        }

        public void InitGrid(GridHexagonData gridHexagon)
        {
            GameObject grid = Instantiate(_GridPrefab, _GridContainer);
            Vector3 cellPos = _Grid.CellToWorld(new Vector3Int(gridHexagon.Row, gridHexagon.Column, 0));
            grid.transform.position = cellPos;
            grid.GetComponentInChildren<TMP_Text>().text = $"R{gridHexagon.Row} : C{gridHexagon.Column}";
        }

        private float GetZCenterPosition(Vector3 maxHeight, Vector3 minHeight)
        {
            Vector3 center = Vector3.Lerp(minHeight, maxHeight, 0.5f);
            return center.z;
        }

        private float GetXCenterPosition(Vector3 maxWidth, Vector3 minWidth)
        {
            Vector3 center = Vector3.Lerp(minWidth, maxWidth, 0.5f);
            return center.x;
        }
    }
}
