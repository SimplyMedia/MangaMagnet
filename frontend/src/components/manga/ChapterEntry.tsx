import React from "react";
import { ChapterMetadataResponse } from "@/services/openapi";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDownload } from "@fortawesome/free-solid-svg-icons";

export default function ChapterEntry({chapter}: { chapter: ChapterMetadataResponse }) {
	return (
		<div key={chapter.id} className={"p-2 px-4 bg-gray-900 odd:bg-gray-800 grid gap-8 grid-cols-3"}>
			<span className={""}>Ch. {chapter.chapterNumber}</span>
			<span className={""}>{chapter.title ?? `Chapter ${chapter.chapterNumber}`}</span>
			<FontAwesomeIcon icon={faDownload}></FontAwesomeIcon>
		</div>
	)
}
