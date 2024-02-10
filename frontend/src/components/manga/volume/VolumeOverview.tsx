import { ChapterMetadataResponse, MangaResponse } from "@/services/openapi";
import React, { useEffect, useState } from "react";
import VolumeList from "@/components/manga/volume/VolumeList";

const groupBy = <T, K extends keyof any>(list: T[], getKey: (item: T) => K) =>
	list.reduce((previous, currentItem) => {
		const group = getKey(currentItem);
		if (!previous[group]) previous[group] = [];
		previous[group].push(currentItem);
		return previous;
	}, {} as Record<K, T[]>);


export default function VolumeOverview({manga}: { manga: MangaResponse }) {
	const [items, setItems] = useState<ChapterMetadataResponse[][]>([]);

	useEffect(() => {
		const groups = groupBy(manga.chapterMetadata, r => r.volumeNumber ?? -1);
		const items = Object.values(groups);

		items.sort((a, b) => (b[0]?.volumeNumber ?? Number.MAX_VALUE) - (a[1]?.volumeNumber ?? Number.MAX_VALUE))

		for (const array of items) {
			array.sort((a, b) => b.chapterNumber - a.chapterNumber);
		}

		setItems(items)
	}, [manga.chapterMetadata]);

	return (
		<div>
			{items.map(i => <VolumeList key={i[0].volumeNumber} chapters={i}/>)}
		</div>
	);
}
