export interface PostFormApiModel {
  id: number;
  title: string;
  text: string;
  imageFiles: Blob[];
  performServiceOnDatesList: Date[];
  performServiceInPlace: string;
}
