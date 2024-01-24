/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ChapterMetadataResult } from '../models/ChapterMetadataResult';
import type { MangaMetadataResponse } from '../models/MangaMetadataResponse';
import type { MangaSearchMetadataResult } from '../models/MangaSearchMetadataResult';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class MetadataService {
    /**
     * @param query
     * @param xVersion
     * @param v
     * @returns MangaSearchMetadataResult Success
     * @throws ApiError
     */
    public static metadataSearch(
        query?: string,
        xVersion?: string,
        v?: string,
    ): CancelablePromise<Array<MangaSearchMetadataResult>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Metadata/search',
            headers: {
                'X-Version': xVersion,
            },
            query: {
                'query': query,
                'v': v,
            },
        });
    }
    /**
     * @param mangaDexId
     * @param xVersion
     * @param v
     * @returns MangaSearchMetadataResult Success
     * @throws ApiError
     */
    public static metadataGet(
        mangaDexId: string,
        xVersion?: string,
        v?: string,
    ): CancelablePromise<MangaSearchMetadataResult> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Metadata/{mangaDexId}',
            path: {
                'mangaDexId': mangaDexId,
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
     * @param mangaDexId
     * @param xVersion
     * @param v
     * @returns MangaSearchMetadataResult Success
     * @throws ApiError
     */
    public static metadataUpdate(
        mangaDexId: string,
        xVersion?: string,
        v?: string,
    ): CancelablePromise<MangaSearchMetadataResult> {
        return __request(OpenAPI, {
            method: 'PATCH',
            url: '/api/Metadata/{mangaDexId}',
            path: {
                'mangaDexId': mangaDexId,
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
     * @param mangaDexId
     * @param xVersion
     * @param v
     * @returns ChapterMetadataResult Success
     * @throws ApiError
     */
    public static metadataGetAllChapters(
        mangaDexId: string,
        xVersion?: string,
        v?: string,
    ): CancelablePromise<Array<ChapterMetadataResult>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Metadata/{mangaDexId}/chapters',
            path: {
                'mangaDexId': mangaDexId,
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
     * @param xVersion
     * @param v
     * @param requestBody
     * @returns MangaMetadataResponse Success
     * @throws ApiError
     */
    public static metadataUpdateBatch(
        xVersion?: string,
        v?: string,
        requestBody?: Array<string>,
    ): CancelablePromise<Array<MangaMetadataResponse>> {
        return __request(OpenAPI, {
            method: 'PATCH',
            url: '/api/Metadata/batch',
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
     * @returns MangaMetadataResponse Success
     * @throws ApiError
     */
    public static metadataUpdateAll(
        xVersion?: string,
        v?: string,
    ): CancelablePromise<Array<MangaMetadataResponse>> {
        return __request(OpenAPI, {
            method: 'PATCH',
            url: '/api/Metadata/all',
            headers: {
                'X-Version': xVersion,
            },
            query: {
                'v': v,
            },
        });
    }
}
