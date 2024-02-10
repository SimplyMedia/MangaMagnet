import { Layout } from "@/components/layout/Layout";
import React, { useEffect, useState } from "react";
import { useRouter } from "next/router";
import { MangaResponse, MangaService } from "@/services/openapi";
import VolumeOverview from "@/components/manga/volume/VolumeOverview";
import MangaHeader from "@/components/manga/MangaHeader";

export default function MangaPage() {
	const [manga, setManga] = useState<MangaResponse | null>(null);
	const {query: {id}} = useRouter();

	useEffect(() => {
		if (!id)
			return;

		MangaService.mangaGetSingle(id as string)
			.then(data => setManga(data))
			.catch(error => console.error('Error fetching data:', error));

	}, [id]);

	if (!manga) {
		return (
			<Layout>We do be loading</Layout>
		)
	}

	return (
		<Layout>
			<MangaHeader manga={manga}/>
			<VolumeOverview manga={manga}/>
		</Layout>
	)
}
