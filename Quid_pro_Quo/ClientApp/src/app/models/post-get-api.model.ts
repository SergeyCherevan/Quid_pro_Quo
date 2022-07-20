export interface PostGetApiModel {
  id: string;
  title: string;
  text: string;
  imageFileNames: string;
  authorName: string;
  postedAt: Date;
  isActual: boolean;
  performServiceOnDatesList: Date[];
  performServiceInPlaceLat: number;
  performServiceInPlaceLng: number;
  performServiceInPlaceZoom: number;
}
