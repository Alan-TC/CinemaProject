import { IMovieImagesUrl } from "./movies-images-url.interface";

export interface IMovie {
    id: string;
    title: string;
    sinopse: string;
    duration: string;
    releaseDate: Date;
    filmGenres: string[];
    classInd: string;
    imagesUrl: IMovieImagesUrl;
}
