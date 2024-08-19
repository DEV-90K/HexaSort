using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GUI_ScreenMain
{
    public class Gallery : MonoBehaviour
    {
        [SerializeField]
        private GameObject _Active;
        [SerializeField]
        private Button _Button;

        [SerializeField]
        private GameObject _Deactive;

        private int _idGallery;

        private void Start()
        {
            _Button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _Button.onClick.RemoveListener(OnButtonClick);
        }

        public void OnInit(int idGallery)
        {
            _idGallery = idGallery;

            GalleryRelicData[] galleryRelics = MainPlayer.Instance.GetGalleryRelicByID(idGallery);
            if(galleryRelics.Length > 0 || idGallery == 1)
            {
                _Active.gameObject.SetActive(true);
                _Deactive.gameObject.SetActive(false);
            }
            else
            {
                if(CheckFullRelics(idGallery - 1))
                {
                    _Active.gameObject.SetActive(true);
                    _Deactive.gameObject.SetActive(false);
                }
                else
                {
                    _Active.gameObject.SetActive(false);
                    _Deactive.gameObject.SetActive(true);
                }
            }
        }

        private bool CheckFullRelics(int idGallery)
        {
            GalleryData galleryData = ResourceManager.Instance.GetGalleryDataByID(idGallery);
            GalleryRelicData[] galleryRelicDatas = MainPlayer.Instance.GetGalleryRelicByID(idGallery);

            if(galleryData.IDRelics.Length == galleryRelicDatas.Length)
            {
                return true;
            }

            return false;
        }

        private void OnButtonClick()
        {
            GUIManager.Instance.ShowPopup<PopupGallery>(_idGallery);
        }
    }
}
