import { Marker, Popup } from "react-leaflet";
import { IMapMarkerProps } from "./IMapMarkerProps";
import styles from "./MapMarker.module.scss";
import Constants from "../../../../../common/utils/constants";
import LikeButton from "../../../../../components/like-button/LikeButton";
import { useNavigate } from "react-router-dom";
import { Routes } from "../../../../../common/enums";

const MapMarker: React.FC<IMapMarkerProps> = (props) => {
  const { event, onLikeClick } = props;

  const navigate = useNavigate();

  const handlePopupClick = (eventId: string) => {
    navigate(Routes.Events + "/" + eventId);
  };

  return (
    <Marker
      key={event.id}
      position={[event.address.point.x, event.address.point.y]}
    >
      <Popup>
        <div
          className={styles.conatiner}
          onClick={() => handlePopupClick(event.id)}
        >
          <img
            src={
              event.imageUrls.length > 0
                ? event.imageUrls[0]
                : Constants.Strings.ImagePlaceholder
            }
            alt="Event"
            className={styles.image}
          />
          <div className={styles.content}>
            <div className={styles.title}>{event.title}</div>
            <div className={styles.description}>{event.description}</div>
            <LikeButton
              id={event.id}
              likes={event.likes}
              isLiked={event.isLiked}
              onLike={onLikeClick}
            />
          </div>
        </div>
      </Popup>
    </Marker>
  );
};

export default MapMarker;
