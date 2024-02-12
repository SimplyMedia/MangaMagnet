import { ChapterData } from "@/components/manga/volume/VolumeOverview";
import { TableCell, TableRow } from "@/components/ui/table";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDownload } from "@fortawesome/free-solid-svg-icons";
import React from "react";
import { MangaService } from "@/services/openapi";

export function ChapterColumn({chapter}: { chapter: ChapterData }) {
	const download = () => {
		MangaService.mangaDownloadSingle(chapter.mangaId, chapter.metadata.chapterNumber);
	}

	return (
		<TableRow>
			<TableCell>{chapter.metadata.chapterNumber}</TableCell>
			<TableCell>{chapter.metadata.title ?? `Chapter ${chapter.metadata.chapterNumber}`}</TableCell>
			<TableCell>{chapter.metadata.createdAt}</TableCell>
			<TableCell>WEBDL-1080p</TableCell>
			<TableCell>
				<FontAwesomeIcon icon={faDownload} className="text-white cursor-pointer" onClick={download}/>
			</TableCell>
		</TableRow>
	)
}
