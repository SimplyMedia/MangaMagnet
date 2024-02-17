import { ChapterData } from "@/components/manga/volume/VolumeOverview";
import React from "react";
import { VolumeResponse } from "@/services/openapi";

export enum LocalFilesDisplay {
	None,
	Partial,
	All
}

export const statusColors: {
	[key in LocalFilesDisplay]: {
		bgClass: string
	}
} = {
	[LocalFilesDisplay.None]: {bgClass: "bg-red-500"},
	[LocalFilesDisplay.Partial]: {bgClass: "bg-yellow-500"},
	[LocalFilesDisplay.All]: {bgClass: "bg-green-500"},
}

export function VolumeLocalFiles({chapters, volume}: { chapters: ChapterData[], volume?: VolumeResponse }) {
	const localChapters = chapters.filter(c => c.local).length ?? 0;
	const hasLocalVolume = volume != undefined;

	const localFileDisplay = hasLocalVolume
		? LocalFilesDisplay.All
		: localChapters === chapters.length
			? LocalFilesDisplay.All
			: localChapters === 0
				? LocalFilesDisplay.None
				: LocalFilesDisplay.Partial;

	const backgroundColor = statusColors[localFileDisplay].bgClass;

	const localCount = hasLocalVolume ? 1 : localChapters;
	const max = hasLocalVolume ? 1 : chapters.length;

	return (
		<span className={`${backgroundColor} px-2 py-1 rounded`}>{localCount}/{max}</span>
	)
}
