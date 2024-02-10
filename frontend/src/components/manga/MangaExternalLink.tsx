import { ExternalLink } from "@/components/ExternalLink";
import React from "react";

export enum MangaExternalLinkType {
	MangaDex,
	AniList,
	MangaUpdates,
	MyAnimeList,
}

export default function MangaExternalLink({id, type}: { id?: string | number | null, type: MangaExternalLinkType }) {
	let url = "";
	let name = "Unknown";

	switch (type) {
		case MangaExternalLinkType.MangaDex:
			name = "MangaDex";
			url = `https://mangadex.org/title/${id}`;
			break;
		case MangaExternalLinkType.AniList:
			name = "Anilist";
			url = `https://anilist.co/manga/${id}`;
			break;
		case MangaExternalLinkType.MangaUpdates:
			name = "MangaUpdates";
			if (typeof id === "string") {
				url = isNaN(parseInt(id))
					? `https://www.mangaupdates.com/series/${id}`
					: `https://www.mangaupdates.com/series.html?id=${id}`;
			}
			break;
		case MangaExternalLinkType.MyAnimeList:
			name = "MyAnimeList";
			url = `https://myanimelist.net/manga/${id}`;
			break;
	}

	return id && url != "" && (
		<ExternalLink
			className={"ml-2"}
			href={url}>
			{name}
		</ExternalLink>
	)
}
