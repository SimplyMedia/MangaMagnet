import { MangaResponse } from "@/services/openapi";
import { statusColors } from "@/components/manga/MangaCard";
import Image from "next/image";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCalculator, faFolder } from "@fortawesome/free-solid-svg-icons";
import Markdown from "react-markdown";
import { ExternalLink } from "@/components/ExternalLink";
import React from "react";

function formatSizeUnits(bytes: number) {
	if (bytes >= 1073741824) {
		return (bytes / 1073741824).toFixed(2) + " GB";
	} else if (bytes >= 1048576) {
		return (bytes / 1048576).toFixed(2) + " MB";
	} else if (bytes >= 1024) {
		return (bytes / 1024).toFixed(2) + " KB";
	} else if (bytes > 1) {
		return bytes + " bytes";
	} else if (bytes == 1) {
		return bytes + " byte";
	} else {
		return "0 bytes";
	}
}

export default function MangaHeader({manga}: { manga: MangaResponse }) {
	let description = manga.metadata.description;

	let index = description.indexOf('---')

	if (index === -1) {
		index = description.indexOf('___')
	}

	if (index !== -1) {
		description = description.substring(0, index);
	}

	const statusInformation = statusColors[manga.metadata.status];

	const volumeSizeInBytes = manga.volumes.reduce((v, c) => v + c.sizeInBytes, 0);
	const chapterSizeInBytes = manga.chapters.reduce((v, c) => v + c.sizeInBytes, 0);

	return (
		<div className={"relative bg-dark overflow-hidden flex"}>
			<div className={"absolute top-0 left-0 right-0 bottom-0 bg-amber-100 card-image"} style={{
				backgroundImage: `url('${manga.metadata.coverImageUrl}')`,
			}}>
			</div>
			<div className={"relative p-5 z-10 flex flex-1"}>
				<div className={"flex flex-1 space-x-4"}>
					<Image
						src={manga.metadata.coverImageUrl ?? '/placeholder-cover.webp'}
						alt={manga.metadata.displayTitle}
						height="300"
						style={{
							objectFit: "cover",
						}}
						width="200"/>
					<div className={"flex flex-col flex-1"}>
						<div>
							<h1 className={"text-5xl inline-block"}>{manga.metadata.displayTitle}</h1>
							<span className={"text-3xl ml-2"}>{manga.metadata.year}</span>
						</div>
						<div className={"flex space-x-2 mt-4"}>
							<div className={"accent-white p-1 px-2 radius"}
								 style={{...statusInformation.style}}>
								{statusInformation.title}
							</div>
							{manga.metadata.genres.map(g => (
								<div key={g} className={"bg-black accent-white p-1 px-2 radius"}>{g}</div>
							))}
						</div>
						<div className={"mt-2"}>
							<div className={"inline-block bg-gray-800 accent-white p-1 px-2 radius"}>
								<FontAwesomeIcon icon={faFolder}/>
								<span className={"ml-2"}>{manga.path}</span>
							</div>
							<div className={"ml-2 inline-block bg-gray-800 accent-white p-1 px-2 radius"}>
								<FontAwesomeIcon icon={faCalculator}/>
								<span className={"ml-2"}>
                                    {formatSizeUnits(volumeSizeInBytes + chapterSizeInBytes)}
                                </span>
							</div>
						</div>
						<div className={"mt-2 flex-1"}>
							<Markdown>{description}</Markdown>
						</div>
						<div className={"mt-4 flex xl:justify-end"}>
							<ExternalLink href={`https://mangadex.org/title/${manga.metadata.mangaDexId}`}>
								MangaDex
							</ExternalLink>

							{manga.metadata.anilistId && (
								<ExternalLink
									className={"ml-2"}
									href={`https://anilist.co/manga/${manga.metadata.anilistId}`}>
									Anlist
								</ExternalLink>
							)}

							{manga.metadata.mangaUpdatesId && (
								<ExternalLink
									className={"ml-2"}
									href={isNaN(parseInt(manga.metadata.mangaUpdatesId))
										? `https://www.mangaupdates.com/series/${manga.metadata.mangaUpdatesId}`
										: `https://www.mangaupdates.com/series.html?id=${manga.metadata.mangaUpdatesId}`}>
									MangaUpdates
								</ExternalLink>
							)}

							{manga.metadata.myAnimeListId && (
								<ExternalLink
									className={"ml-2"}
									href={`https://myanimelist.net/manga/${manga.metadata.myAnimeListId}`}>
									MyAnimeList
								</ExternalLink>
							)}
						</div>
					</div>
				</div>
			</div>
		</div>
	)
}
