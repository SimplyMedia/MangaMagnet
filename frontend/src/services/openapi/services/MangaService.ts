/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CreateMangaRequest } from '../models/CreateMangaRequest';
import type { MangaResponse } from '../models/MangaResponse';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class MangaService {
    /**
     * @param xVersion
     * @param v
     * @param requestBody
     * @returns MangaResponse Created
     * @throws ApiError
     */
    public static mangaCreate(
        xVersion?: string,
        v?: string,
        requestBody?: CreateMangaRequest,
    ): CancelablePromise<MangaResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Manga',
            headers: {
                'X-Version': xVersion,
            },
            query: {
                'v': v,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param xVersion
     * @param v
     * @returns MangaResponse Success
     * @throws ApiError
     */
    public static mangaGetAll(
        xVersion?: string,
        v?: string,
    ): CancelablePromise<Array<MangaResponse>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Manga',
            headers: {
                'X-Version': xVersion,
            },
            query: {
                'v': v,
            },
        });
    }
    /**
     * @param xVersion
     * @param v
     * @param requestBody
     * @returns MangaResponse Created
     * @throws ApiError
     */
    public static mangaCreateBatch(
        xVersion?: string,
        v?: string,
        requestBody?: Array<CreateMangaRequest>,
    ): CancelablePromise<MangaResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Manga/batch',
            headers: {
                'X-Version': xVersion,
            },
            query: {
                'v': v,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @param xVersion
     * @param v
     * @returns MangaResponse Success
     * @throws ApiError
     */
    public static mangaGetSingle(
        id: string,
        xVersion?: string,
        v?: string,
    ): CancelablePromise<MangaResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Manga/{id}',
            path: {
                'id': id,
            },
            headers: {
                'X-Version': xVersion,
            },
            query: {
                'v': v,
            },
        });
    }
    /**
     * @param id
     * @param xVersion
     * @param v
     * @returns MangaResponse Success
     * @throws ApiError
     */
    public static mangaDelete(
        id: string,
        xVersion?: string,
        v?: string,
    ): CancelablePromise<MangaResponse> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Manga/{id}',
            path: {
                'id': id,
            },
            headers: {
                'X-Version': xVersion,
            },
            query: {
                'v': v,
            },
        });
    }
}
